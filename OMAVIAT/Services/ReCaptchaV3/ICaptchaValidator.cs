namespace OMAVIAT.Services.ReCaptchaV3
{
	public interface ICaptchaV3Validator
	{
		Task<bool> IsCaptchaPassedAsync(string token);
	}
}
