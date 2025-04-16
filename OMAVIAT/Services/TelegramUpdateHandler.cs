using NLog;
using OMAVIAT.Entities.Telegram;
using OMAVIAT.Schedule.Utilities;
using OMAVIAT.Services.News;
using OMAVIAT.Utilities.Telegram;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OMAVIAT.Services;

public class TelegramUpdateHandler : IUpdateHandler
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	private static List<TelegramMediaGroup> MediaGroups { get; set; } = [];
	public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source,
		CancellationToken token)
	{
		try
		{
			var errorMessage = exception switch
			{
				ApiRequestException apiRequestException
					=> $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
				_ => exception.ToString()
			};

			Logger.Error(errorMessage);
		}
		catch (Exception ex)
		{
			Console.WriteLine(
				$"При обработке ошибки произошла ошибка.\nПервоначальная ошибка: {exception}\nОшибка при обработке: {ex}\n ");
		}

		return Task.CompletedTask;
	}


	public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update,
		CancellationToken cancellationToken)
	{
		try
		{
			switch (update.Type)
			{
				case UpdateType.CallbackQuery:
					await TelegramBot.BotClient.SendMessage(update.CallbackQuery!.Message!.Chat.Id,
						update.CallbackQuery!.Data!, cancellationToken: cancellationToken);
					break;
				case UpdateType.ChannelPost:
					if (update.ChannelPost?.Caption is not null && (update.ChannelPost.MediaGroupId is not null ||
					                                                (update.ChannelPost.MediaGroupId is null &&
					                                                 update.ChannelPost.Photo is not null)))

					{
						if (update.ChannelPost.MediaGroupId is null && update.ChannelPost.Photo is not null)
						{
							Runs.InThread(async () =>
							{
								await TelegramPublishNewsHandler.OnNewTelegramMessage(update,
									[new(update.ChannelPost.MessageId, update.ChannelPost.Photo!.Last())]);
							});
							return;
						}

						var mediaGroup =
							MediaGroups.FirstOrDefault(e => e.MediaGroupId == update.ChannelPost.MediaGroupId);
						if (mediaGroup is null)
						{
							mediaGroup = new TelegramMediaGroup(update.ChannelPost.MediaGroupId!,
								[new(update.ChannelPost.MessageId,  update.ChannelPost.Photo!.Last())]);
							MediaGroups.Add(mediaGroup);
						}
						else
							mediaGroup.Photos.Add(new(update.ChannelPost.MessageId, update.ChannelPost.Photo!.Last()));

						Runs.InThread(async () =>
						{
							var localMediaId = update.ChannelPost.MediaGroupId;
							await Task.Delay(new TimeSpan(0, 0, 5), cancellationToken); //change 
							await TelegramPublishNewsHandler.OnNewTelegramMessage(update,
								MediaGroups.FirstOrDefault(e => e.MediaGroupId == localMediaId)?.Photos);
							MediaGroups.Remove(mediaGroup);
						});
					}
					else if (update.ChannelPost?.Caption is null && update.ChannelPost?.MediaGroupId is not null)
					{
						var mediaGroup = MediaGroups.FirstOrDefault(e => e.MediaGroupId == update.ChannelPost.MediaGroupId);
						if (mediaGroup is null)
						{
							 mediaGroup = new TelegramMediaGroup(update.ChannelPost.MediaGroupId, [new(update.ChannelPost.MessageId, update.ChannelPost.Photo!.Last())]); 
							 MediaGroups.Add(mediaGroup);
						}
						else
						{
							var photo = update.ChannelPost.Photo?.LastOrDefault();
							if (photo is not null) mediaGroup.Photos.Add(new(update.ChannelPost.MessageId, photo));
						}
					}
					break;
				case UpdateType.EditedChannelPost:
					await TelegramEditNewsHandler.EditNewsAsync(update);
					break;
			}
		}
		catch (Exception ex)
		{
			Logger.Error($"Произошла ошибка при обработке запроса. Тип: {update.Type} \n{ex}");
			if (update.CallbackQuery is not null)
				Logger.Error($"Текст, который использовался: {update.CallbackQuery.Data ?? ""}");
			if (update.Message?.Text != null)
				Logger.Error($"Текст, который использовался: {update.Message.Text}");
		}
	}
}