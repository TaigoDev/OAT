using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OAT.UtilsHelper.ReCaptcha;

namespace OAT.Modules.ReCaptchaV3
{

	public class CaptchaVerificationService
	{


		public static async Task<bool> IsCaptchaValid(string token)
		{
			var result = false;

			var googleVerificationUrl = "https://www.google.com/recaptcha/api/siteverify";

			try
			{
				using var client = new HttpClient();

				var response = await client.PostAsync($"{googleVerificationUrl}?secret={Configurator.ReCaptchaV2.CodeSecretKey}&response={token}", null);
				var jsonString = await response.Content.ReadAsStringAsync();
				var captchaVerfication = JsonConvert.DeserializeObject<ReCaptchaResponse>(jsonString);

				result = captchaVerfication.Success;
			}
			catch (Exception e)
			{
				Logger.Error(e);
			}

			return result;
		}
	}
}
