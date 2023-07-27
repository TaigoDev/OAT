using System.Runtime.InteropServices;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace OAT.Utilities
{
    public class Telegram
    {

        protected static TelegramConfig config = new TelegramConfig();
        protected static TelegramBotClient botClient;

        public static async void init()
        {
            try
            {
                config = Utils.SetupConfiguration("telegram.yml", new TelegramConfig());
                botClient = new TelegramBotClient(config.token, new HttpClient());
                var me = await botClient.GetMeAsync();
                Console.WriteLine($"Авторизация бота Telegram успешно произошла. Имя бота: {me.Username}");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }


        public static async void SendMessage(string message)
        {
            try
            {
                if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                    await botClient.SendTextMessageAsync(new ChatId(int.Parse(config.chat_id)), message);
                else
                    Console.WriteLine(message);
            }
            catch { }
        }

    }
}
