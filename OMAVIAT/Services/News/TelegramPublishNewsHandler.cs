using System.Net.Http.Headers;
using System.Text;
using NLog;
using OMAVIAT.Entities.Telegram;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Model;
using Message = Telegram.Bot.Types.Message;
using PhotoSize = Telegram.Bot.Types.PhotoSize;

namespace OMAVIAT.Services.News;

public static class TelegramPublishNewsHandler
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();


	public static async Task OnNewTelegramMessage(Update update, List<TelegramPhoto>? photos)
	{
		if(!ValidateTelegramChannel(update) || photos is null) return;
		if(update.ChannelPost is null) return;
		
		var news = CreateNews(update.ChannelPost, await DownloadPhotos(update, photos));
		if(news is null) return;
		
		await PublishOnSite(news, update, update.ChannelPost);
		await TelegramRepostNews(news, update);
		await VkRepostNews(news, update);
	}


	private static async Task VkRepostNews(Entities.Database.News news, Update update)
	{
		if(!IsElo(update) || !news.description.Contains(Configurator.EloNewsPublishTag)) return;
		var api = new VkApi();
		await api.AuthorizeAsync(new ApiAuthParams
		{
			AccessToken = Configurator.Config.Telegram.EloVkApiKey
			
		});
		var attachments = new List<MediaAttachment>();
		var uploadServer = api.Photo.GetWallUploadServer(Configurator.Config.Telegram.EloVkGroupId);
		foreach (var photo in news.photos.toObject<List<string>>())
		{
			var response = await UploadFile(uploadServer.UploadUrl,
				Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo));

			var attachment = api.Photo.SaveWallPhoto(response, Configurator.Config.Telegram.EloVkUserId, (ulong)Configurator.Config.Telegram.EloVkGroupId).FirstOrDefault();
			if (attachment is not null)
				attachments.Add(attachment);
		}

		var message = update.ChannelPost;
		if(message is null) return;
		var replacements = new List<Replacement>();
		for (var id = 0; id < message.CaptionEntities?.Length; id++)
		{
			var entity = message.CaptionEntities[id];
			var value = message.CaptionEntityValues!.ToArray()[id];
			if(entity.Type == MessageEntityType.TextLink)
				replacements.Add(new Replacement( entity.Offset, entity.Length, $"{value} (ссылка: {entity.Url})"));
		}

		var description = ReplaceMultiple(message.Caption!, replacements);
		var post = await api.Wall.PostAsync(new WallPostParams()
		{
			OwnerId = -Configurator.Config.Telegram.EloVkGroupId, 
			Message = description,
			Attachments = attachments
		});
		await using var db = new DatabaseContext();
		news.EloVkPostId = post;
		db.Update(news);
		await db.SaveChangesAsync();
	}
	

	private static async Task TelegramRepostNews(Entities.Database.News news, Update update)
	{
		if(!IsElo(update) || !news.description.Contains(Configurator.NewsPublishTag)) return;
		await using var db = new DatabaseContext();
		var mediaGroup = new List<IAlbumInputMedia>();
		var photos = news.photos.toObject<List<string>>();
		if (photos.Count == 0) return;
		var photo = photos.First();
		mediaGroup.Add(new InputMediaPhoto(InputFile.FromStream(
			File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo)), $"{StringUtils.RandomString(5)}.{Path.GetExtension(photo)}"))
		{
			Caption = news.description,
			ParseMode = ParseMode.Html,
		});
		mediaGroup.AddRange(photos.Skip(1)
			.Select(path =>
				new InputMediaPhoto(InputFile.FromStream(
					File.OpenRead(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path)), $"{StringUtils.RandomString(5)}.{Path.GetExtension(path)}"))));
		var message =
			await TelegramBot.BotClient.SendMediaGroup(new ChatId(Configurator.Config.Telegram.NewsChannelId),
				mediaGroup);
		news.TelegramMessageId = message.FirstOrDefault()?.MessageId;
		news.TelegramMediaGroupId = message.FirstOrDefault()?.MediaGroupId;
		db.Update(news);
		await db.SaveChangesAsync();
	}
	
	
	private static async Task PublishOnSite(Entities.Database.News news, Update update, Message message)
	{
		await using var db = new DatabaseContext();
		if(IsElo(update) && !news.description.Contains(Configurator.NewsPublishTag)) return;
		
		if (IsElo(update))
		{
			news.EloTelegramMessageId = message.MessageId;
			news.EloTelegramMediaGroupId = message.MediaGroupId;
		}
		else
		{
			news.TelegramMessageId = message.MessageId;
			news.TelegramMediaGroupId = message.MediaGroupId;
		}

		await db.AddAsync(news);
		await db.SaveChangesAsync();
		Log(news);
		await NewsReader.Init();
	}
	
	private static Entities.Database.News? CreateNews(Message message, List<string> photos)
	{
		if(message.Caption is null || message.CaptionEntityValues is null) return null;
		var replacements = new List<Replacement>();
		for (var id = 0; id < message.CaptionEntities?.Length; id++)
		{
			var entity = message.CaptionEntities[id];
			var value = message.CaptionEntityValues.ToArray()[id];
			if(entity.Type == MessageEntityType.TextLink)
				replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{entity.Url}\">{value}</a>"));
			else if (entity.Type == MessageEntityType.Url)
				replacements.Add(new Replacement( entity.Offset, entity.Length, $"<a href=\"{value}\">{value}</a>"));
		}

		var description = ReplaceMultiple(message.Caption, replacements);
		var title = message.Caption.Split("\n").FirstOrDefault() ?? message.Caption.GetWords(10);
		if (title.Length > 100)
			title = title.GetWords(10);
		return new Entities.Database.News(DateTime.Now.ToString("yyyy-MM-dd"),
			title, description, message.Caption.GetWords(15), photos, false);
	}

	private static async Task<List<string>> DownloadPhotos(Update update, List<TelegramPhoto> photoSizes)
	{
		var urls = new List<string>();
		foreach (var photo in photoSizes)
		{
			var downloaded = await DownloadMedia(update, photo);
			if(downloaded is not null) urls.Add(downloaded); 
		}
		return urls;
	}
	
	private static bool IsElo(Update update) => Configurator.Config.Telegram.NewsEloChannelId == update.GetChatId();
	
	private static async Task<string?> DownloadMedia(Update update, TelegramPhoto photo)
	{
		var path = $"images/news/{StringUtils.SHA226($"{photo.Photo.FileUniqueId}")}-{StringUtils.SHA226($"telegram-msg-id-{photo.MessageId}")}.png";
		return await DownloadImage(photo.Photo.FileId, Path.Combine("wwwroot", path)) ? path : null;
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
		             [TelegramNewsService]: Новость на сайте была опубликована автоматически, т.к. в сообщение официального ТГ канала ОМАВИАТ ({Configurator.Config.Telegram.NewsChannelId}) был указан хештег ({Configurator.NewsPublishTag})
		             Информация об опубликованной новости:
		             ID: {news.id}
		             Title: {news.title}
		             Description: {news.description}
		             Date: {news.date}
		             Photos count: {news.photos.toObject<List<string>>().Count}
		             MessageId: {news.TelegramMessageId ?? news.EloTelegramMessageId}
		             """);;
	}
	private static bool ValidateTelegramChannel(Update update)
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
			return false;
		}

		if (update.ChannelPost?.Caption is not null && update.ChannelPost.CaptionEntityValues is not null &&
		    update.ChannelPost.CaptionEntityValues.Contains(Configurator.NewsPublishTag)) return true;
		Logger.Info($"""
		             Telegram Chat: {update.GetChatId()}
		             Config Telegram Chat: {Configurator.Config.Telegram.NewsChannelId}
		             Caption: {update.ChannelPost?.Caption}
		             CaptionEntityValues nullable: {update.ChannelPost?.CaptionEntityValues is null}
		             PublishTag: {Configurator.NewsPublishTag}
		             """);
		return false;
	}
}
