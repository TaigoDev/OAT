using System.Net;
using NLog;
using OMAVIAT.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace OMAVIAT.Utilities.Telegram;

public static class TelegramBot
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	public static TelegramBotClient BotClient { get; set; }
	public static bool IsProxy { get; set; }
	public static CancellationTokenSource CancelTokenSource { get; set; } = new();

	public static async Task Init()
	{
		try
		{
			CancelTokenSource = new CancellationTokenSource();
			if (!await IsWorkWithoutProxy())
			{
				var proxy = new WebProxy
				{
					Address = new Uri("http://10.0.55.52:3128"),
					BypassProxyOnLocal = false,
					UseDefaultCredentials = false
				};

				var httpClientHandler = new HttpClientHandler
				{
					Proxy = proxy,
					ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
					{
						return true;
					}
				};
				IsProxy = true;
				BotClient = new TelegramBotClient(Configurator.Config.Telegram.ApiKey,
					new HttpClient(httpClientHandler));
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
				BotClient = new TelegramBotClient(Configurator.Config.Telegram.ApiKey, new HttpClient());
				IsProxy = false;
			}

			if (!Configurator.Config.IsProduction)
				return;
			var me = await BotClient.GetMe();
			var receiverOptions = new ReceiverOptions
			{
				AllowedUpdates = []
			};
			BotClient.StartReceiving(new TelegramUpdateHandler());
			Logger.Info($"Авторизация бота Telegram успешно произошла. Имя бота: {me.Username}");
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
			var response =
				await client.GetAsync($"https://api.telegram.org/bot{Configurator.Config.Telegram.ApiKey}/getMe");
			return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NotFound or HttpStatusCode.Unauthorized;
		}
		catch (Exception ex)
		{
			Console.WriteLine(ex.Message);
			return false;
		}
	}

	public static async Task SendMessage(string message)
	{
		try
		{
			if (Configurator.Config.IsProduction)
				await BotClient.SendMessage(new ChatId(Configurator.Config.Telegram.LogChatId), message);
		}
		catch (Exception ex)
		{
			if (ex.HResult != -2146233088)
				Logger.Error(ex);
		}
	}


	public static async void SendMessage(string message, Exception errorNotification)
	{
		try
		{
			if (Configurator.Config.IsProduction)
				await BotClient.SendMessage(new ChatId(Configurator.Config.Telegram.LogChatId), message,
					replyMarkup: Buttons.CreateKeyboard(Buttons.CreateButtonInRow("🔓 Посмотреть ошибку",
						$"{errorNotification.Message}")));
			else
				Console.WriteLine(message);
		}
		catch (Exception ex)
		{
			if (ex.HResult != -2146233088)
				Logger.Error(ex);
		}
	}
}