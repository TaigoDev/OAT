using Newtonsoft.Json.Linq;
using System.Net;

namespace OAT.Modules.ReCaptchaV3
{
	public class ReCaptchaV3Validator : ICaptchaV3Validator
	{
		private const string RemoteAddress = "https://www.google.com/recaptcha/api/siteverify";
		private readonly string _secretKey;
		private readonly double acceptableScore;
		private readonly IHttpClientFactory _httpClientFactory;

		public ReCaptchaV3Validator(IHttpClientFactory httpClient)
		{
			_httpClientFactory = httpClient;
			_secretKey = Configurator.ReCaptchaV3.CodeSecretKey;
			acceptableScore = Configurator.ReCaptchaV3.AcceptableScore;
		}

		public async Task<bool> IsCaptchaPassedAsync(string token)
		{
			dynamic response = await GetCaptchaResultDataAsync(token);
			if (response.success == "true")
			{
				return System.Convert.ToDouble(response.score) >= acceptableScore;
			}
			return false;
		}

		public async Task<JObject> GetCaptchaResultDataAsync(string token)
		{
			var content = new FormUrlEncodedContent(new[]
			{
				new KeyValuePair<string, string>("secret", _secretKey),
				new KeyValuePair<string, string>("response", token)
			});
			using var httpClient = _httpClientFactory.CreateClient();
			var res = await httpClient.PostAsync(RemoteAddress, content);
			if (res.StatusCode != HttpStatusCode.OK)
			{
				throw new HttpRequestException(res.ReasonPhrase);
			}
			var jsonResult = await res.Content.ReadAsStringAsync();
			return JObject.Parse(jsonResult);
		}
	}
}
