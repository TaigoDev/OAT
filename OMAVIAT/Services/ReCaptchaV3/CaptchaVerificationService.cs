using Newtonsoft.Json;
using OAT;
using OMAVIAT.Services.ReCaptchaV2;
using System.Net;

namespace OMAVIAT.Services.ReCaptchaV3
{

	public class CaptchaVerificationService
	{


		public static async Task<bool> IsCaptchaValid(string token)
		{
			var result = false;

			var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

			try
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

				var response = await client.PostAsync($"{googleVerificationUrl}?secret={Configurator.ReCaptchaV2.CodeSecretKey}&response={token}", null);
				var jsonString = await response.Content.ReadAsStringAsync();
				var captchaVerfication = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);

				result = captchaVerfication is not null && captchaVerfication.Success;
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}

			return result;
		}
	}
}
