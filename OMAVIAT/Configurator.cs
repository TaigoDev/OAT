using System.Runtime.InteropServices;
using OMAVIAT.Entities.Configs;
using OMAVIAT.Utilities;

namespace OMAVIAT;

public static class Configurator
{
	public static string NewsPublishTag { get; set; } = "#омавиат_сайт"; 
	public static string EloTelegramRepostPublishTag { get; set; } = "#омавиат_тг";
	public static string VkPublishTag { get; set; } = "#омавиат_вк";
	public static string EloVkPublishTag { get; set; } = "#лицей_вк";

	public static Config Config { get; set; } = new();
	public static TimeTableBotConfig Timetable { get; set; } = new();
	public static TimeTableBotConfig MyOat { get; set; } = new();
	public static YandexSmartCaptchaConfig SmartCaptcha { get; set; } = new();
	public static MailConfig MailConfig { get; set; } = new();

	public static async Task Init()
	{
		Config = await FileUtils.SetupConfiguration(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources",
				RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || OperatingSystem.IsMacOS()
					? "config.yml"
					: "config-linux.yml"), new Config());
		
		Timetable = await FileUtils.SetupConfiguration(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "timetable_bot.yml"), new TimeTableBotConfig());
		
		MyOat = await FileUtils.SetupConfiguration(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "myoat.yml"), new TimeTableBotConfig());

		SmartCaptcha = await FileUtils.SetupConfiguration(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "smartcaptcha.yml"),
			new YandexSmartCaptchaConfig());
		
		MailConfig = await FileUtils.SetupConfiguration(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "mail.yml"),
			new MailConfig());
	}
}