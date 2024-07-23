using System.Text.Json.Serialization;

namespace OMAVIAT.Services.ReCaptchaV2
{
	public class ReCaptchaResponse
	{
		[JsonPropertyName("success")]
		public bool Success { get; set; }
		[JsonPropertyName("score")]
		public double Score { get; set; }
		[JsonPropertyName("action")]
		public string Action { get; set; }
		[JsonPropertyName("challenge_ts")]
		public DateTime ChallengeTimestamp { get; set; }
		[JsonPropertyName("hostname")]
		public string Hostname { get; set; }
		[JsonPropertyName("error-codes")]
		public string[] ErrorCodes { get; set; }
	}
}
