using System.Net;
using NLog;
using OMAVIAT.Utilities.Telegram;

namespace OMAVIAT.Utilities;

public class DownDetector
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static Task Init()
	{
		new Task(async void () =>
		{
			try
			{
				while (true)
				{
					if (!await CheckConnection())
					{
						await TelegramBot.CancelTokenSource.CancelAsync();
						await TelegramBot.Init();
						Logger.Warn("⚠️ Канал связи был изменен автоматически");
					}

					await Task.Delay(TimeSpan.FromMinutes(15));
				}
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}
		}).Start();
		return Task.CompletedTask;
	}


	private static async Task<bool> CheckConnection()
	{
		try
		{
			if (TelegramBot.IsProxy)
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
					ServerCertificateCustomValidationCallback = (_, _, _, _) => true
				};
				using var client = new HttpClient(httpClientHandler);
				var response =
					await client.GetAsync($"https://api.telegram.org/bot{Configurator.Config.Telegram.ApiKey}/getMe");
				return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NotFound
					or HttpStatusCode.Unauthorized;
			}
			else
			{
				using var client = new HttpClient();
				var response =
					await client.GetAsync($"https://api.telegram.org/bot{Configurator.Config.Telegram.ApiKey}/getMe");
				return response.StatusCode is HttpStatusCode.OK or HttpStatusCode.NotFound
					or HttpStatusCode.Unauthorized;
			}
		}
		catch (Exception ex)
		{
			Console.WriteLine(
				$"Произошла ошибка подключения к https://api.telegram.org/bot{Configurator.Config.Telegram.ApiKey}/getMe. Прокси: {TelegramBot.IsProxy}. Ошибка: {ex}");
			return false;
		}
	}
}