using Newtonsoft.Json.Linq;
using System.Net;

namespace OAT.Controllers.ReCaptchaV3
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
				return Convert.ToDouble(response.score) >= acceptableScore;
			}
			return false;
		}

		public async Task<JObject> GetCaptchaResultDataAsync(string token)
		{
			try
			{
				var content = new FormUrlEncodedContent(new[]
				{
				new KeyValuePair<string, string>("secret", _secretKey),
				new KeyValuePair<string, string>("response", token)
			});

				var proxy = new WebProxy
				{
					Address = new Uri($"http://10.0.55.52:3128"),
					BypassProxyOnLocal = false,
					UseDefaultCredentials = false,
				};

				var httpClientHandler = new HttpClientHandler
				{
					Proxy = proxy,
				};
				httpClientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
				using var httpClient = new HttpClient(httpClientHandler);
				var res = await httpClient.PostAsync(RemoteAddress, content);
				if (res.StatusCode != HttpStatusCode.OK)
				{
					throw new HttpRequestException(res.ReasonPhrase);
				}
				var jsonResult = await res.Content.ReadAsStringAsync();
				return JObject.Parse(jsonResult);
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return new();
			}
		}
	}
}
