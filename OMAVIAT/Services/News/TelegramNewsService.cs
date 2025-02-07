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
		Logger.Info($"ChatId {update.GetChatId()}");
		Logger.Info($"Caption {update.ChannelPost?.Caption}");
		Logger.Info($"1 {update.GetChatId() != Configurator.Config.Telegram.NewsChannelId}");
		Logger.Info($"2 {update.ChannelPost?.Caption is null}");
		Logger.Info($"3 {!update.ChannelPost?.CaptionEntityValues?.Contains(Configurator.Config.Telegram.NewsPublishTag)}");

		if (update.GetChatId() != Configurator.Config.Telegram.NewsChannelId
		    || update.ChannelPost?.Caption is null || update.ChannelPost.CaptionEntityValues is null ||
		    !update.ChannelPost.CaptionEntityValues.Contains(Configurator.Config.Telegram.NewsPublishTag))
		{
			Logger.Info("Exit");
			return;
		}
		Logger.Info("Download images...");

		var photos = new List<string>();
		foreach (var photo in update.ChannelPost.Photo ?? [])
		{
			var path = $"images/news/{StringUtils.SHA226($"{photo.FileId}-{photo.FileUniqueId}")}.png";
			if (await DownloadImage(photo.FileId, Path.Combine("wwwroot", path)))
				photos.Add(path);
		}
		Logger.Info("Download images... COMPLETE");

		if (photos.Count == 0)
		{
			Logger.Info(
				$"[TelegramNewsService]: Не удалось найти фото, чтобы выложить новость {update.ChannelPost.Id} из {Configurator.Config.Telegram.NewsChannelId}");
			return;
		}
		Logger.Info("Create news");

		var title = update.ChannelPost.Caption.Split("\n").FirstOrDefault() ?? update.ChannelPost.Caption.GetWords(10);
		if (title.Length > 50)
			title = title.GetWords(10);
		await using var connection = new DatabaseContext();
		var news = new Entities.Database.News(DateTime.Now.ToString("yyyy-MM-dd"),
			title, update.ChannelPost.Caption, update.ChannelPost.Caption.GetWords(15), photos, false);
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