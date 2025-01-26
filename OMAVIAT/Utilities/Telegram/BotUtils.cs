using NLog;
using Telegram.Bot.Types;

namespace OMAVIAT.Utilities.Telegram;

public static class BotUtils
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static long GetChatId(this Update update)
	{
		if (update.ChannelPost is not null && update.ChannelPost.Chat is not null)
			return update.ChannelPost.Chat.Id;

		if (update.Message is not null && update.Message.Chat is not null)
			return update.Message.Chat.Id;

		if (update.Message is not null && update.Message.From is not null)
			return update.Message.From.Id;

		if (update.CallbackQuery is not null && update.CallbackQuery.Message is not null)
			return update.CallbackQuery.Message.Chat.Id;

		if (update.CallbackQuery is not null && update.CallbackQuery.From is not null)
			return update.CallbackQuery.From.Id;


		Logger.Warn("Не удалось определить id чата отправителя");
		return 0;
	}
}