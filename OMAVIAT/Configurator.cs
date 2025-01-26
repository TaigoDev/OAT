using System.Runtime.InteropServices;
using OMAVIAT.Entities.Configs;
using OMAVIAT.Utilities;

namespace OMAVIAT;

public class Configurator
{
	public static Config Config { get; set; } = new();
	public static TimeTableBotConfig Timetable { get; set; } = new();
	public static TimeTableBotConfig MyOat { get; set; } = new();
	public static YandexSmartCaptchaConfig SmartCaptcha { get; set; } = new();

	public static async Task Init()
	{
		Config = await FileUtils.SetupConfiguration<Config>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources",
				RuntimeInformation.IsOSPlatform(OSPlatform.Windows) || OperatingSystem.IsMacOS()
					? "config.yml"
					: "config-linux.yml"), new Config());
		Timetable = await FileUtils.SetupConfiguration<TimeTableBotConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "timetable_bot.yml"), new TimeTableBotConfig());
		MyOat = await FileUtils.SetupConfiguration<TimeTableBotConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "myoat.yml"), new TimeTableBotConfig());

		SmartCaptcha = await FileUtils.SetupConfiguration<YandexSmartCaptchaConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "smartcaptcha.yml"),
			new YandexSmartCaptchaConfig());
	}
}