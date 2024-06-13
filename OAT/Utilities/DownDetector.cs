using NPOI.SS.Formula.Functions;
using OAT.Utilities.Telegram;
using System.Net;

namespace OAT.Utilities
{
	public class DownDetector
	{
		public static void init()
		{
			new Task(async () =>
			{
				try
				{
					while (true)
					{
						if (!await CheckConnection())
						{
							TelegramBot.cancelTokenSource.Cancel();
							await TelegramBot.init();
							Logger.Warning("⚠️ Канал связи был изменен автоматически");
						}
						await Task.Delay(TimeSpan.FromMinutes(15));
					}
				}
				catch (Exception e)
				{
					Logger.Error(e);
				}
			}).Start();
		}


		private static async Task<bool> CheckConnection()
		{
			try
			{
				if (TelegramBot.IsProxy)
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
					using var client = new HttpClient(httpClientHandler);
					var response = await client.GetAsync($"https://api.telegram.org/bot{Configurator.telegram.token}/getMe");
					if (response.StatusCode is not System.Net.HttpStatusCode.OK && response.StatusCode is not HttpStatusCode.NotFound && response.StatusCode is not HttpStatusCode.Unauthorized)
						return false;
					return true;

				}
				else
				{
					using var client = new HttpClient();
					var response = await client.GetAsync($"https://api.telegram.org/bot{Configurator.telegram.token}/getMe");
					if (response.StatusCode is not System.Net.HttpStatusCode.OK && response.StatusCode is not HttpStatusCode.NotFound && response.StatusCode is not HttpStatusCode.Unauthorized)
						return false;
					return true;
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return false;
			}
		}
	}
}
