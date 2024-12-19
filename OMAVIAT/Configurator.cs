using OMAVIAT.Entities.Configs;
using OMAVIAT.Utilities;
using System.Runtime.InteropServices;

namespace OMAVIAT {
	public class Configurator {

		public static Config config = new();
		public static TelegramConfig telegram = new();
		public static TimeTableBotConfig timetable = new();
		public static TimeTableBotConfig myoat = new();
		public static YandexSmartCaptchaConfig SmartCaptcha = new();
		public static bool IsLocal = OperatingSystem.IsWindows();

		public static async Task init()
		{
			config = await FileUtils.SetupConfiguration<Config>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "config.yml" : "config-linux.yml"), new());
			telegram = await FileUtils.SetupConfiguration<TelegramConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "telegram.yml"), new());
			timetable = await FileUtils.SetupConfiguration<TimeTableBotConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "timetable_bot.yml"), new());
			myoat = await FileUtils.SetupConfiguration<TimeTableBotConfig>(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "myoat.yml"), new());

			SmartCaptcha = await FileUtils.SetupConfiguration<YandexSmartCaptchaConfig>(
			Path.Combine(Directory.GetCurrentDirectory(), "Resources", "smartcaptcha.yml"), new());
		}



	}
}
