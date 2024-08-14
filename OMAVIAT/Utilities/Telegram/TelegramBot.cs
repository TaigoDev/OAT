using System.Net;
using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace OMAVIAT.Utilities.Telegram
{
	public class TelegramBot
	{

		protected static TelegramBotClient botClient;
		public static bool IsProxy = false;
		public static CancellationTokenSource cancelTokenSource = new CancellationTokenSource();
		public static async Task init()
		{
			try
			{

				cancelTokenSource = new CancellationTokenSource();
				if (!await IsWorkWithoutProxy())
				{
					var proxy = new WebProxy
					{
						Address = new Uri($"http://10.0.55.52:3128"),
						BypassProxyOnLocal = false,
						UseDefaultCredentials = false,
					};

					var httpClientHandler = new HttpClientHandler
					{
						Proxy = proxy,
						ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
					};
					IsProxy = true;
					botClient = new TelegramBotClient(Configurator.telegram.token, new HttpClient(httpClientHandler));
					try
					{
						Logger.Info("⚠️ ПОДКЛЮЧЕНИЕ ЧЕРЕЗ РЕЗЕРВНЫЙ КАНАЛ СВЯЗИ");
					}
					catch (Exception ex)
					{
						Console.WriteLine(ex.ToString());
					}
				}
				else
				{ 
					botClient = new TelegramBotClient(Configurator.telegram.token, new HttpClient());
					IsProxy = false;
				}

				if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					return;
				var me = await botClient.GetMeAsync();
				var receiverOptions = new ReceiverOptions() { AllowedUpdates = Array.Empty<UpdateType>() };
				botClient.StartReceiving(
				updateHandler: HandleUpdateAsync,
					pollingErrorHandler: HandlePollingErrorAsync,
					receiverOptions: receiverOptions,
					cancellationToken: cancelTokenSource.Token
				);
				Logger.InfoWithoutTelegram($"Авторизация бота Telegram успешно произошла. Имя бота: {me.Username}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex);
			}
		}

		private static async Task<bool> IsWorkWithoutProxy()
		{
			try
			{
				using var client = new HttpClient();
				var response = await client.GetAsync($"https://api.telegram.org/bot{Configurator.telegram.token}/getMe");
				if (response.StatusCode is not System.Net.HttpStatusCode.OK && response.StatusCode is not HttpStatusCode.NotFound && response.StatusCode is not HttpStatusCode.Unauthorized)
					return false;
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
		public static async void SendMessage(string message)
		{
			try
			{
				if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					await botClient.SendTextMessageAsync(new ChatId(long.Parse(Configurator.telegram.chat_id)), message);
				else
					Console.WriteLine(message);
			}
			catch (Exception ex)
			{
				if (ex.HResult != -2146233088)
					Logger.Error(ex);
			}
		}


		public static async void SendMessage(string message, Exception ErrorNotification)
		{
			try
			{
				if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
					await botClient.SendTextMessageAsync(new ChatId(long.Parse(Configurator.telegram.chat_id)), message,
						replyMarkup: Buttons.CreateKeyboard(Buttons.CreateButtonInRow("🔓 Посмотреть ошибку", $"{ErrorNotification.Message}")));
				else
					Console.WriteLine(message);
			}
			catch (Exception ex)
			{
				if (ex.HResult != -2146233088)
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
			if (exception.HResult != -2146233088)
				Logger.Error(ErrorMessage);
			return Task.CompletedTask;
		}

	}
}
