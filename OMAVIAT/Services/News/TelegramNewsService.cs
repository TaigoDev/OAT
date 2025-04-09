using Microsoft.EntityFrameworkCore;
using NLog;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Quartz;
using Quartz.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Logger = NLog.Logger;

namespace OMAVIAT.Services.News;

public static class TelegramNewsService
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task OnNewMessage(Update update)
	{
		if (update.GetChatId() != Configurator.Config.Telegram.NewsChannelId &&
		    update.GetChatId() != Configurator.Config.Telegram.NewsEloChannelId)
		{
			Logger.Info($"""
			             Telegram Chat: {update.GetChatId()}
			             Config Telegram Chat: {Configurator.Config.Telegram.NewsChannelId}
			             Caption: {update.ChannelPost?.Caption}
			             CaptionEntityValues nullable: {update.ChannelPost?.CaptionEntityValues is null}
			             PublishTag: {Configurator.NewsPublishTag}
			             """);
			return;
		}
		
		var isElo = Configurator.Config.Telegram.NewsEloChannelId == update.GetChatId();
		await using var connection = new DatabaseContext();
		if (update.ChannelPost?.MediaGroupId is not null && update.ChannelPost?.Caption is null)
		{
			if(update.ChannelPost is null) return;
			var mediaNews = isElo ? 
				await connection.News.FirstOrDefaultAsync(e => e.EloTelegramMediaGroupId == update.ChannelPost.MediaGroupId) 
				: await connection.News.FirstOrDefaultAsync(e => e.TelegramMediaGroupId == update.ChannelPost.MediaGroupId);
			if(mediaNews is null) return;
			var media = await DownloadMedia(update);
			if(media is null) return;
			var currentMedia = mediaNews.GetPhotos();
			currentMedia.Add(media);
			mediaNews.photos = currentMedia.toJson();
			connection.Update(mediaNews);
			await connection.SaveChangesAsync();
			await NewsReader.Init();
			return;
		}

		if (update.ChannelPost?.Caption is null || update.ChannelPost.CaptionEntityValues is null ||
		    !update.ChannelPost.CaptionEntityValues.Contains(Configurator.NewsPublishTag))
		{
			
			Logger.Info($"""
						Telegram Chat: {update.GetChatId()}
						Config Telegram Chat: {Configurator.Config.Telegram.NewsChannelId}
						Caption: {update.ChannelPost?.Caption}
						CaptionEntityValues nullable: {update.ChannelPost?.CaptionEntityValues is null}
						PublishTag: {Configurator.NewsPublishTag}
						""");
			return;
		}


		var photos = new List<string>();
		var photo = await DownloadMedia(update);
		if(photo is not null) photos.Add(photo); 

		if (photos.Count == 0)
		{
			Logger.Info(
				$"[TelegramNewsService]: Не удалось найти фото, чтобы выложить новость {update.ChannelPost.Id} из {Configurator.Config.Telegram.NewsChannelId}");
			return;
		}
		var urls = update.ChannelPost.CaptionEntityValues.Where(e => e.Contains("http://") || e.Contains("https://")).ToList();
		var title = update.ChannelPost.Caption.Split("\n").FirstOrDefault() ?? update.ChannelPost.Caption.GetWords(10);
		if (title.Length > 50)
			title = title.GetWords(10);
		var description = urls.Aggregate(update.ChannelPost.Caption, (current, url) => current.Replace(url, $"<a href=\"{url}\">{url}</a>"));
		var news = new Entities.Database.News(DateTime.Now.ToString("yyyy-MM-dd"),
			title, description, update.ChannelPost.Caption.GetWords(15), photos, false);
		if (isElo)
		{
			news.EloTelegramMessageId = update.ChannelPost.MessageId;
			news.EloTelegramMediaGroupId = update.ChannelPost.MediaGroupId;
		}
		else
		{
			news.TelegramMessageId = update.ChannelPost.MessageId;
			news.TelegramMediaGroupId = update.ChannelPost.MediaGroupId;
		}
		

		await connection.AddAsync(news);
		await connection.SaveChangesAsync();
		if(isElo)
		{			
			var job = JobBuilder.Create<RepostNewsJob>()
				.WithIdentity("PostNewsFromEloGroup", "TelegramNewsService").UsingJobData("newsId", news.id)
				.Build();
			var trigger = TriggerBuilder.Create()
				.WithIdentity("ExecuteLater", "TelegramNewsService")
				.StartAt(DateTimeOffset.Now.AddSeconds(10)) 
				.Build();
			await ScheduleService.Scheduler.ScheduleJob(job, trigger);
			await ScheduleService.Scheduler.Start();

		}
		Logger.Info($"""
		             [TelegramNewsService]: Новость на сайте была опубликована автоматически, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.NewsPublishTag})
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
			if (update.GetChatId() != Configurator.Config.Telegram.NewsChannelId &&
			    update.GetChatId() != Configurator.Config.Telegram.NewsEloChannelId)
			{
				Logger.Info($"""
				             Telegram Chat: {update.GetChatId()}
				             Config Telegram Chat: {Configurator.Config.Telegram.NewsChannelId}
				             Caption: {update.ChannelPost?.Caption}
				             CaptionEntityValues nullable: {update.ChannelPost?.CaptionEntityValues is null}
				             PublishTag: {Configurator.NewsPublishTag}
				             """);
				return;
			}

			var isElo = Configurator.Config.Telegram.NewsEloChannelId == update.GetChatId();

			if (
			    update.EditedChannelPost?.MediaGroupId is not null && update.EditedChannelPost?.Caption is null)
			{
				if(update.EditedChannelPost?.Photo is null) return;
				var mediaNews = isElo ? 
					await connection.News.FirstOrDefaultAsync(e => e.EloTelegramMediaGroupId == update.EditedChannelPost.MediaGroupId) 
					: await connection.News.FirstOrDefaultAsync(e => e.TelegramMediaGroupId == update.EditedChannelPost.MediaGroupId);				if(mediaNews is null) return;
				var currentPhoto = update.EditedChannelPost.Photo.LastOrDefault();
				if(currentPhoto is null) return;
				
				var currentMedia = mediaNews.GetPhotos();
				currentMedia.RemoveAll(e => e.Contains(StringUtils.SHA226($"telegram-msg-id-{update.EditedChannelPost?.MessageId}")));
				currentMedia.Add(await DownloadMedia(update));
				mediaNews.photos = currentMedia.toJson();
				connection.Update(mediaNews);
				await connection.SaveChangesAsync();
				await NewsReader.Init();
				return;
			}
			if (update.EditedChannelPost?.Caption is null || update.EditedChannelPost.CaptionEntityValues is null ||
			    !update.EditedChannelPost.CaptionEntityValues.Contains(Configurator.NewsPublishTag))
				return;
			var news = isElo ? await connection.News.FirstOrDefaultAsync(e =>
				e.EloTelegramMessageId == update.EditedChannelPost.MessageId) :
					await connection.News.FirstOrDefaultAsync(e =>
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
			var urls = update.EditedChannelPost.CaptionEntityValues.Where(e => e.Contains("http://") || e.Contains("https://")).ToList();
			var description = urls.Aggregate(update.EditedChannelPost.Caption, (current, url) => current.Replace(url, $"<a href=\"{url}\">{url}</a>"));
			news.description = description;
			news.short_description = update.EditedChannelPost.Caption.GetWords(15);
			connection.Update(news);
			await connection.SaveChangesAsync();
			if(isElo && news.TelegramMessageId is not null)
				await TelegramBot.BotClient.EditMessageCaption(new ChatId(Configurator.Config.Telegram.NewsChannelId),(int)news.TelegramMessageId, description, parseMode: ParseMode.Html);
			

			Logger.Info($"""
			             [TelegramNewsService]: Новость на сайте была ОБНОВЛЕНА автоматически, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.NewsPublishTag})
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


	private static async Task<string?> DownloadMedia(Update update)
	{
		if(update.ChannelPost?.Photo is null && update.EditedChannelPost?.Photo is null)
			return null;
		
		var photo = (update.ChannelPost?.Photo ?? update.EditedChannelPost?.Photo!).LastOrDefault();
		if (photo is null) return null;
		var path = $"images/news/{StringUtils.SHA226($"{photo.FileUniqueId}")}-{StringUtils.SHA226($"telegram-msg-id-{update.ChannelPost?.MessageId ?? update.EditedChannelPost?.MessageId}")}.png";

		return await DownloadImage(photo.FileId, Path.Combine("wwwroot", path)) ? path : null;
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