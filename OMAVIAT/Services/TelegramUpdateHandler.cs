using NLog;
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
						update.CallbackQuery!.Data, cancellationToken: cancellationToken);
					break;
				case UpdateType.ChannelPost:
					await TelegramNewsService.OnNewMessage(update);
					break;
				case UpdateType.EditedChannelPost:
					await TelegramNewsService.OnEditMessage(update);
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