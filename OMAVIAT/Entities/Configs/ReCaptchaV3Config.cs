namespace OMAVIAT.Entities.Configs
{
	public class ReCaptchaV3Config
	{
		public string CodeSecretKey { get; set; } = "6Lc1GpMpAAAAABINM13YmJF8lTb5YywXiqeB4ai4";
		public string WebSecretKey { get; set; } = "6Lc1GpMpAAAAAB_4lNiPzuIGmQ6Zrb8gEgms4Gd9";
		public double AcceptableScore { get; set; } = 0.5;
	}
}
