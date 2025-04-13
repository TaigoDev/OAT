using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Quartz;
using Quartz.Impl;
using Quartz.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using VkNet;
using VkNet.Model;

namespace OMAVIAT.Services.News;

public class RepostNewsJob : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		
		await using var db = new DatabaseContext();
		var newsId = context.JobDetail.JobDataMap.GetIntValue("newsId");
		var news = await db.News.FindAsync(newsId);
		if (news is null) return;
		await PublishEloVkonakte(news);
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

	public static async Task PublishEloVkonakte(Entities.Database.News news)
	{
		var api = new VkApi();
		await api.AuthorizeAsync(new ApiAuthParams
		{
			AccessToken = "vk1.a.uq2xNIbdB71-2f3BPJkpvyC-atFl6C3Be4GW-zXHOlrIPbeCK_zHNR3Hi6v3sDCj2alzYm5rLY2HrXlm8mzw5nGu6-c9xIID0tW2RMw5R9Xxn5pEC34fXtTFMpwlWB3sk-PH8tI0i_vLdrUVUkFNRtIP2A-Pb_OkqI7NonP3aeD1Oa6UG3CDPHapVc4MDvkrOChlm4bTYnlXrdfTzLj8Xw",
		});
		var attachments = new List<MediaAttachment>();
		var uploadServer = api.Photo.GetWallUploadServer(-216259673);
		foreach (var photo in news.photos.toObject<List<string>>())
		{
			var response = await UploadFile(uploadServer.UploadUrl,
				Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", photo));

			var attachment = api.Photo.Save(new PhotoSaveParams()
			{
				SaveFileResponse = response,
			}).FirstOrDefault();
			if (attachment is not null)
				attachments.Add(attachment);
		}
		
		await api.Wall.PostAsync(new WallPostParams()
		{
			OwnerId = -216259673, 
			Message = news.description,
			Attachments = attachments
		});
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


}