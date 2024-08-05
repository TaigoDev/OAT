using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace OMAVIAT.Utilities.Telegram
{
	public static class MessageController
	{


		public static async Task EditMessageOrNewAsync(this ITelegramBotClient botClient,
			ChatId chatId, string text, Update update, int? MessageThreadId = null,
			ParseMode? parseMode = null, IEnumerable<MessageEntity>? entities = null,
			bool? disableWebPagePreview = null, bool? disableNotification = null,
			bool? ProtectContent = null, int? replyToMessageId = null,
			bool? allowSendingWithoutReply = null, InlineKeyboardMarkup? replyMarkup = null,
			CancellationToken cancellationToken = default)
		{
			if (update.CallbackQuery is not null && update.CallbackQuery.Message is not null)
				try
				{

					await botClient.EditMessageTextAsync(chatId, update.CallbackQuery.Message.MessageId, text, parseMode, entities,
					disableWebPagePreview,
					replyMarkup, cancellationToken);
					return;
				}
				catch
				{
				}

			await botClient.SendTextMessageAsync(chatId, text, MessageThreadId, parseMode, entities,
				disableWebPagePreview, disableNotification, ProtectContent, replyToMessageId, allowSendingWithoutReply,
				replyMarkup, cancellationToken);
		}

	}
}
