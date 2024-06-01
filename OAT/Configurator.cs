using Microsoft.AspNetCore.Components;
using OAT.Entities.Configs;
using OAT.Utilities;
using System.Runtime.InteropServices;

namespace OAT
{
	public class Configurator
	{

		public static Config config = new();
		public static TelegramConfig telegram = new();
		public static TimeTableBotConfig timetable = new();
		public static ReCaptchaV3Config ReCaptchaV3 = new();
		public static ReCaptchaV2Config ReCaptchaV2 = new();
		public static OldMysql old_mysql = new();

		public static async Task init()
		{
			config = await FileUtils.SetupConfiguration<Config>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "config.yml" : "config-linux.yml"), new());
			telegram = await FileUtils.SetupConfiguration<TelegramConfig>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", "telegram.yml"), new());
			timetable = await FileUtils.SetupConfiguration<TimeTableBotConfig>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", "timetable_bot.yml"), new());
			ReCaptchaV3 = await FileUtils.SetupConfiguration<ReCaptchaV3Config>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ReCaptchaV3.yml"), new());
			ReCaptchaV2 = await FileUtils.SetupConfiguration<ReCaptchaV2Config>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", "ReCaptchaV2.yml"), new());
			old_mysql = await FileUtils.SetupConfiguration<OldMysql>(
				Path.Combine(Directory.GetCurrentDirectory(), "Resources", "old_mysql.yml"), new());
		}



	}
}
