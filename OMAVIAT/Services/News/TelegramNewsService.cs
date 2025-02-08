using Microsoft.EntityFrameworkCore;
using NLog;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OMAVIAT.Services.News;

public static class TelegramNewsService
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task OnNewMessage(Update update)
	{
		await using var connection = new DatabaseContext();
		if (update.GetChatId() == Configurator.Config.Telegram.NewsChannelId &&
		    update.ChannelPost?.MediaGroupId is not null && update.ChannelPost?.Caption is null)
		{
			if(update.ChannelPost is null) return;
			var mediaNews = await connection.News.FirstOrDefaultAsync(e => e.TelegramMediaGroupId == update.ChannelPost.MediaGroupId);
			if(mediaNews is null) return;
			var media = await DownloadMedia(update);
			var currentMedia = mediaNews.GetPhotos();
			currentMedia.AddRange(media);
			mediaNews.photos = currentMedia.toJson();
			connection.Update(mediaNews);
			await connection.SaveChangesAsync();
			await NewsReader.Init();
			return;
		}
		
		if (update.GetChatId() != Configurator.Config.Telegram.NewsChannelId
		    || update.ChannelPost?.Caption is null || update.ChannelPost.CaptionEntityValues is null ||
		    !update.ChannelPost.CaptionEntityValues.Contains(Configurator.Config.Telegram.NewsPublishTag))
			return;
		var photos = await DownloadMedia(update);
		if (photos.Count == 0)
		{
			Logger.Info(
				$"[TelegramNewsService]: Не удалось найти фото, чтобы выложить новость {update.ChannelPost.Id} из {Configurator.Config.Telegram.NewsChannelId}");
			return;
		}

		var title = update.ChannelPost.Caption.Split("\n").FirstOrDefault() ?? update.ChannelPost.Caption.GetWords(10);
		if (title.Length > 50)
			title = title.GetWords(10);
		var news = new Entities.Database.News(DateTime.Now.ToString("yyyy-MM-dd"),
			title, update.ChannelPost.Caption, update.ChannelPost.Caption.GetWords(15), photos, false);
		news.TelegramMessageId = update.ChannelPost.MessageId;
		news.TelegramMediaGroupId = update.ChannelPost.MediaGroupId;
		await connection.AddAsync(news);
		await connection.SaveChangesAsync();

		Logger.Info($"""
		             [TelegramNewsService]: Новость на сайте была опубликована автоматически, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.Config.Telegram.NewsPublishTag})
		             Информация об опубликованной новости:
		             ID: {news.id}
		             Title: {news.title}
		             Description: {news.description}
		             Date: {news.date}
		             Photos count: {photos.Count}
		             MessageId: {update.ChannelPost.MessageId}
		             """);
		await NewsReader.Init();
	}


	public static async Task OnEditMessage(Update update)
	{
		try
		{
			await using var connection = new DatabaseContext();

			if (update.GetChatId() == Configurator.Config.Telegram.NewsChannelId &&
			    update.EditedChannelPost?.MediaGroupId is not null && update.EditedChannelPost?.Caption is null)
			{
				if(update.EditedChannelPost?.Photo is null) return;
				var mediaNews = await connection.News.FirstOrDefaultAsync(e => e.TelegramMediaGroupId == update.EditedChannelPost.MediaGroupId);
				if(mediaNews is null) return;
				var currentPhoto = update.EditedChannelPost.Photo.LastOrDefault();
				if(currentPhoto is null) return;
				
				var currentMedia = mediaNews.GetPhotos();
				currentMedia.RemoveAll(e => e.Contains(StringUtils.SHA226($"telegram-msg-id-{update.EditedChannelPost?.MessageId}")));
				currentMedia.AddRange(await DownloadMedia(update));
				mediaNews.photos = currentMedia.toJson();
				connection.Update(mediaNews);
				await connection.SaveChangesAsync();
				await NewsReader.Init();
				return;
			}
			if (update.GetChatId() != Configurator.Config.Telegram.NewsChannelId
			    || update.EditedChannelPost?.Caption is null || update.EditedChannelPost.CaptionEntityValues is null ||
			    !update.EditedChannelPost.CaptionEntityValues.Contains(Configurator.Config.Telegram.NewsPublishTag))
				return;
			var news = await connection.News.FirstOrDefaultAsync(e =>
				e.TelegramMessageId == update.EditedChannelPost.MessageId);
			if (news is null) return;
			if (update.EditedChannelPost.Photo is null || update.EditedChannelPost.Photo.Length == 0)
			{
				Logger.Info(
					$"[TelegramNewsService]: Не удалось найти фото, чтобы выложить новость {update.EditedChannelPost.Id} из {Configurator.Config.Telegram.NewsChannelId}");
				return;
			}

			var photo = update.EditedChannelPost.Photo.LastOrDefault();
			var photos = news.GetPhotos();

			if (photo is not null)
			{
				photos.RemoveAll(e => e.Contains(StringUtils.SHA226($"telegram-msg-id-{update.EditedChannelPost?.MessageId}")));
				photos.AddRange(await DownloadMedia(update));
			}
			

			var title = update.EditedChannelPost.Caption.Split("\n").FirstOrDefault() ??
			            update.EditedChannelPost.Caption.GetWords(10);
			if (title.Length > 50)
				title = title.GetWords(10);
			news.title = title;
			news.photos = photos.toJson();
			news.description = update.EditedChannelPost.Caption;
			news.short_description = update.EditedChannelPost.Caption.GetWords(15);
			connection.Update(news);
			await connection.SaveChangesAsync();

			Logger.Info($"""
			             [TelegramNewsService]: Новость на сайте была ОБНОВЛЕНА автоматически, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.Config.Telegram.NewsPublishTag})
			             Информация об опубликованной новости:
			             ID: {news.id}
			             Title: {news.title}
			             Description: {news.description}
			             Date: {news.date}
			             MessageId: {update.EditedChannelPost.MessageId}
			             """);
			await NewsReader.Init();
		}
		catch (Exception ex)
		{
			Logger.Error(ex);
		}

	}


	private static async Task<List<string>> DownloadMedia(Update update)
	{
		var photos = new List<string>();
		if(update.ChannelPost?.Photo is null && update.EditedChannelPost?.Photo is null)
			return [];
		
		var photo = (update.ChannelPost?.Photo ?? update.EditedChannelPost?.Photo!).LastOrDefault();
		if (photo is null) return [];
		var path = $"images/news/{StringUtils.SHA226($"{photo.FileUniqueId}")}-{StringUtils.SHA226($"telegram-msg-id-{update.ChannelPost?.MessageId ?? update.EditedChannelPost?.MessageId}")}.png";
		if (await DownloadImage(photo.FileId, Path.Combine("wwwroot", path)))
			photos.Add(path);
		return photos;
	}
	
	private static async Task<bool> DownloadImage(string fileId, string path)
	{
		try
		{
			var file = await TelegramBot.BotClient.GetFile(fileId);
			if (file.FilePath is null) return false;
			await using var fs = new FileStream(path, FileMode.Create);
			await TelegramBot.BotClient.DownloadFile(file.FilePath, fs);
			return true;
		}
		catch (Exception e)
		{
			Logger.Error(e);
			return false;
		}
	}
}