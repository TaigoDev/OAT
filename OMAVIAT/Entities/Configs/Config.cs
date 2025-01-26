namespace OMAVIAT.Entities.Configs;

public class Config
{
	public bool IsProduction { get; set; } = false;
	public bool BitrixProxy { get; set; }
	public string BaseUrl { get; set; } = "https://www.oat.ru/";
	public string MainUrl { get; set; } = "https://www.oat.ru/";
	public int BindPort { get; set; } = 20045;
	public DatabaseConfig Database { get; set; } = new();
	public LdapConfig Ldap { get; set; } = new();
	public TelegramLoggerConfig Telegram { get; set; } = new();
}