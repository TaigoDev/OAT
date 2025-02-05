namespace OMAVIAT.Entities.Configs;

public class MailConfig
{
	public string SmtpServer { get; set; } = "smtp.yandex.ru";
	public int SmtpPort { get; set; } = 465;
	public bool EnableSsl { get; set; } = true;
	public string EmailUser { get; set; } = "dod@oat.ru";
	public string EmailPassword { get; set; } = "Pa$swOrD";
	public string SendTo { get; set; } = "dod@oat.ru";
	public bool EnableProxy { get; set; } = true;

}