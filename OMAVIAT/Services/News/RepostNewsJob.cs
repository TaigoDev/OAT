using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Quartz;
using Quartz.Impl;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OMAVIAT.Services.News;

public class RepostNewsJob : IJob
{
	public async Task Execute(IJobExecutionContext context)
	{
		
		await using var db = new DatabaseContext();
		var newsId = context.JobDetail.JobDataMap.GetIntValue("newsId");
		var news = await db.News.FindAsync(newsId);
		if (news is null) return;
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
}