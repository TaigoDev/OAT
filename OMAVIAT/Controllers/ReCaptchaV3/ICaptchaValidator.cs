namespace OAT.Controllers.ReCaptchaV3
{
	public interface ICaptchaV3Validator
	{
		Task<bool> IsCaptchaPassedAsync(string token);
	}
}
