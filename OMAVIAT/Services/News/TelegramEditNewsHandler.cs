using System.Net.Http.Headers;
using System.Text;
using Microsoft.EntityFrameworkCore;
using NLog;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Model;
using PhotoSize = Telegram.Bot.Types.PhotoSize;

namespace OMAVIAT.Services.News;

public static class TelegramEditNewsHandler
{
	
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task EditNewsAsync(Update update)
	{
		await EloNewsAsync(update);
		await OmaviatNewsAsync(update);
	}

	private static async Task OmaviatNewsAsync(Update update)
	{
		if(update.EditedChannelPost is null)
			return;
		await using var db = new DatabaseContext();
		var news = await db.News.FirstOrDefaultAsync(e => e.TelegramMediaGroupId == update.EditedChannelPost.MediaGroupId 
		                                                  || e.TelegramMessageId == update.EditedChannelPost.MessageId);
		if(news is null) return;
		
		if (update.EditedChannelPost.Photo is not null)
		{
			var currentMedia = news.GetPhotos();
			currentMedia.RemoveAll(e => e.Contains(StringUtils.SHA226($"telegram-msg-id-{update.EditedChannelPost.MessageId}")));
			var photo = await DownloadMedia(update, update.EditedChannelPost.Photo.Last());
			if(photo is null) return;
			currentMedia.Add(photo);
			news.photos = currentMedia.toJson();
		}

		if (update.EditedChannelPost.Caption is not null)
		{
			var message = update.EditedChannelPost;
			if(message is null) return;
			var replacements = new List<Replacement>();
			for (var id = 0; id < message.CaptionEntities?.Length; id++)
			{
				var entity = message.CaptionEntities[id];
				var value = message.CaptionEntityValues!.ToArray()[id];
				if(entity.Type == MessageEntityType.TextLink)
					replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{entity.Url}\">{value}</a>"));
				else if (entity.Type == MessageEntityType.Url)
					replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{value}\">{value}</a>"));
			}

			news.description = ReplaceMultiple(message.Caption, replacements);
			news.title = message.Caption.Split("\n").FirstOrDefault() ?? message.Caption.GetWords(10);
			if (news.title.Length > 100)
				news.title = news.title.GetWords(10);
			news.short_description = message.Caption.GetWords(15);
		}

		db.Update(news);
		await db.SaveChangesAsync();
		await VkUpdateAsync(update, news);
		await NewsReader.Init();
		Logger.Info($"Новость #{news.id} обновлена на сайте (скрыта: {news.IsHide})");
		Log(news);
	}
	

	private static async Task EloNewsAsync(Update update)
	{
		if(update.EditedChannelPost is null) return;
		await using var db = new DatabaseContext();
		var news = await db.News.FirstOrDefaultAsync(e => e.EloTelegramMediaGroupId == update.EditedChannelPost.MediaGroupId 
		                                                  || e.EloTelegramMessageId == update.EditedChannelPost.MessageId);
		if(news is null) return;
		
		if (update.EditedChannelPost.Photo is not null)
		{
			var currentMedia = news.GetPhotos();
			currentMedia.RemoveAll(e => e.Contains(StringUtils.SHA226($"telegram-msg-id-{update.EditedChannelPost.MessageId}")));
			var photo = await DownloadMedia(update, update.EditedChannelPost.Photo.Last());
			if(photo is null) return;
			currentMedia.Add(photo);
			news.photos = currentMedia.toJson();
		}

		if (update.EditedChannelPost.Caption is not null)
		{
			var message = update.EditedChannelPost;
			if(message is null) return;
			var replacements = new List<Replacement>();
			for (var id = 0; id < message.CaptionEntities?.Length; id++)
			{
				var entity = message.CaptionEntities[id];
				var value = message.CaptionEntityValues!.ToArray()[id];
				if(entity.Type == MessageEntityType.TextLink)
					replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{entity.Url}\">{value}</a>"));
				else if (entity.Type == MessageEntityType.Url)
					replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{value}\">{value}</a>"));
			}

			news.description = ReplaceMultiple(message.Caption, replacements);
			news.title = message.Caption.Split("\n").FirstOrDefault() ?? message.Caption.GetWords(10);
			if (news.title.Length > 100)
				news.title = news.title.GetWords(10);
			news.short_description = message.Caption.GetWords(15);
		}

		await RepostNewsUpdateAsync(news);
		await EloVkUpdateAsync(update, news);
		await VkUpdateAsync(update, news);
		db.Update(news);
		await db.SaveChangesAsync();
		await NewsReader.Init();
		Logger.Info("Новость с лицея была обновлена на сайте oat.ru");
		Log(news);
	}

	private static async Task VkUpdateAsync(Update update, Entities.Database.News news)
	{
		try
		{
			if (!news.description.Contains(Configurator.VkPublishTag)) return;
			var api = new VkApi();
			await api.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = Configurator.Config.Telegram.VkApiKey
			});
			var attachments = new List<MediaAttachment>();
			var uploadServer = api.Photo.GetWallUploadServer(Configurator.Config.Telegram.VkGroupId);
			foreach (var photo in news.photos.toObject<List<string>>())
			{
				var response = await UploadFile(uploadServer.UploadUrl,
					Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo));

				var attachment = api.Photo.SaveWallPhoto(response, Configurator.Config.Telegram.VkUserId,
					(ulong)Configurator.Config.Telegram.VkGroupId).FirstOrDefault();
				if (attachment is not null)
					attachments.Add(attachment);
			}


			var param = new WallEditParams()
			{
				OwnerId = -Configurator.Config.Telegram.VkGroupId,
				Attachments = attachments,
				PostId = news.VkPostId
			};
			if (update.EditedChannelPost?.Caption is not null)
			{
				var message = update.EditedChannelPost;
				var replacements = new List<Replacement>();
				for (var id = 0; id < message.CaptionEntities?.Length; id++)
				{
					var entity = message.CaptionEntities[id];
					var value = message.CaptionEntityValues!.ToArray()[id];
					if (entity.Type == MessageEntityType.TextLink)
						replacements.Add(new Replacement(entity.Offset, entity.Length,
							$"{value} (ссылка: {entity.Url})"));
				}

				var description = ReplaceMultiple(message.Caption!, replacements);
				param.Message = description;
			}
			else
			{
				var posts = await api.Wall.GetByIdAsync(
					[$"-{Configurator.Config.Telegram.EloVkGroupId}_{news.VkPostId}"], 0);
				var post = posts.FirstOrDefault();
				if (post is null) return;
				param.Message = post.Text;
			}

			await api.Wall.EditAsync(param);
			Logger.Info($"Новость #{news.id} обновлена в ВК омавиата (скрыта: {news.IsHide})");
		}
		catch (Exception e)
		{
			Logger.Error(e.Message);
		}
	}
	
	private static async Task EloVkUpdateAsync(Update update, Entities.Database.News news)
	{
		try
		{
			if (!news.description.Contains(Configurator.EloVkPublishTag)) return;
			var api = new VkApi();
			await api.AuthorizeAsync(new ApiAuthParams
			{
				AccessToken = Configurator.Config.Telegram.VkApiKey
				
			});
			var attachments = new List<MediaAttachment>();
			var uploadServer = api.Photo.GetWallUploadServer(Configurator.Config.Telegram.EloVkGroupId);
			foreach (var photo in news.photos.toObject<List<string>>())
			{
				var response = await UploadFile(uploadServer.UploadUrl,
					Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo));

				var attachment = api.Photo.SaveWallPhoto(response, Configurator.Config.Telegram.VkUserId, (ulong)Configurator.Config.Telegram.EloVkGroupId).FirstOrDefault();
				if (attachment is not null)
					attachments.Add(attachment);
			}
			

			var param = new WallEditParams()
			{
				OwnerId = -Configurator.Config.Telegram.EloVkGroupId,
				Attachments = attachments,
				PostId = news.EloVkPostId
			};
			if (update.EditedChannelPost?.Caption is not null)
			{
				var message = update.EditedChannelPost;
				var replacements = new List<Replacement>();
				for (var id = 0; id < message.CaptionEntities?.Length; id++)
				{
					var entity = message.CaptionEntities[id];
					var value = message.CaptionEntityValues!.ToArray()[id];
					if(entity.Type == MessageEntityType.TextLink)
						replacements.Add(new Replacement( entity.Offset, entity.Length, $"{value} (ссылка: {entity.Url})"));
				}
				var description = ReplaceMultiple(message.Caption!, replacements);
				param.Message = description;
			}
			else
			{
				var posts = await api.Wall.GetByIdAsync([ $"-{Configurator.Config.Telegram.EloVkGroupId}_{news.EloVkPostId}"], 0);
				var post = posts.FirstOrDefault();
				if(post is null) return;
				param.Message = post.Text;
			}
			
			await api.Wall.EditAsync(param);	
			Logger.Info($"Новость #{news.id} обновлена в ВК лицея (скрыта: {news.IsHide})");
		}
		catch (Exception e)
		{
			Logger.Error(e.Message);
		}
	}

	private static async Task RepostNewsUpdateAsync(Entities.Database.News news)
	{
		if(news.TelegramMessageId is null) return;
		try
		{
			await TelegramBot.BotClient.EditMessageCaption(new ChatId(Configurator.Config.Telegram.NewsChannelId),
				(int)news.TelegramMessageId, 
				news.description, parseMode: ParseMode.Html);
			Logger.Info("Фото в пересланном сообщении не было обновлено, т.к. сейчас нет поддержки данного функционала");
		}
		catch 
		{
			//ignored
		}

	}
	
	private static bool IsElo(Update update) => Configurator.Config.Telegram.NewsEloChannelId == update.GetChatId();
	
	private static async Task<string?> DownloadMedia(Update update,PhotoSize photo)
	{
		var path = $"images/news/{StringUtils.SHA226($"{photo.FileUniqueId}")}-{StringUtils.SHA226($"telegram-msg-id-{update.ChannelPost?.MessageId ?? update.EditedChannelPost?.MessageId}")}.png";
		return await DownloadImage(photo.FileId, Path.Combine("wwwroot", path)) ? path : null;
	}

	private record Replacement(int StartIndex, int Length, string NewValue);

	static string ReplaceMultiple(string input, List<Replacement> replacements)
	{
		if (input == null) throw new ArgumentNullException(nameof(input));
		if (replacements == null) throw new ArgumentNullException(nameof(replacements));

		var sorted = replacements.OrderByDescending(r => r.StartIndex);

		foreach (var r in sorted)
		{
			if (r.StartIndex < 0 || r.StartIndex + r.Length > input.Length)
				throw new ArgumentOutOfRangeException($"Invalid range: {r.StartIndex}-{r.Length}");

			input = input.Substring(0, r.StartIndex) + r.NewValue + input.Substring(r.StartIndex + r.Length);
		}

		return input;
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
	
	private static async Task<string> UploadFile(string serverUrl, string file)
	{
		var data = await File.ReadAllBytesAsync(file);
		
		using var client = new HttpClient();
		var requestContent = new MultipartFormDataContent();
		var content = new ByteArrayContent(data);
		
		content.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
		requestContent.Add(content, "file", new FileInfo(file).Name);
		var response = client.PostAsync(serverUrl, requestContent).Result;
		return Encoding.Default.GetString(await response.Content.ReadAsByteArrayAsync());
	}

	private static void Log(Entities.Database.News news)
	{
		Logger.Info($"""
		             [TelegramNewsService]: Новость на сайте была опубликована ОБНОВЛЕНА, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.NewsPublishTag})
		             Информация об опубликованной новости:
		             ID: {news.id}
		             Title: {news.title}
		             Description: {news.description}
		             Date: {news.date}
		             Photos count: {news.photos.toObject<List<string>>().Count}
		             MessageId: {news.TelegramMessageId ?? news.EloTelegramMessageId}
		             """);;
	}
	
}