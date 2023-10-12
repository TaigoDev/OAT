using Microsoft.Extensions.Logging;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OAT.Utilities
{
    public class TelegramBot
    {

        protected static TelegramConfig config = new TelegramConfig();
        protected static TelegramBotClient botClient;

        public static async Task init()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                return;
            config = Utils.SetupConfiguration("telegram.yml", new TelegramConfig());
            botClient = new TelegramBotClient(config.token, new HttpClient());
            var me = await botClient.GetMeAsync();
            var receiverOptions = new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() };
            botClient.StartReceiving(
            updateHandler: HandleUpdateAsync,
                pollingErrorHandler: HandlePollingErrorAsync,
                receiverOptions: receiverOptions,
                cancellationToken: new CancellationTokenSource().Token
            );
            Logger.InfoWithoutTelegram($"Авторизация бота Telegram успешно произошла. Имя бота: {me.Username}");
        }


        public static async void SendMessage(string message)
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    await botClient.SendTextMessageAsync(new ChatId(long.Parse(config.chat_id)), message);
                else
                    Console.WriteLine(message);
            }
            catch(Exception ex) {
                Logger.Error(ex);
            }
        }


        public static async void SendMessage(string message, Exception ErrorNotification)
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    await botClient.SendTextMessageAsync(new ChatId(long.Parse(config.chat_id)), message,
                        replyMarkup: Buttons.CreateKeyboard(Buttons.CreateButtonInRow("🔓 Посмотреть ошибку", $"{ErrorNotification.Message}")));
                else
                    Console.WriteLine(message);
            }
            catch(Exception ex) {
                Logger.Error(ex);
            }
        }

        public static async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            if (update.Type is UpdateType.CallbackQuery)
                await botClient.EditMessageOrNewAsync(update.CallbackQuery!.Message!.Chat.Id, update.CallbackQuery!.Data, update);
        }

        public static Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                    => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Logger.Error(ErrorMessage);
            return Task.CompletedTask;
        }

    }
}
