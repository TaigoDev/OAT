namespace MyOAT.Utilities.Cookies;

public interface ICookie
{
	public Task SetValue(string key, string value, int? days = null);
	public Task<string> GetValue(string key, string def = "");
	public Task DeleteValue(string key);
}