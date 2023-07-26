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
            config = Utils.SetupConfiguration("telegram.yml", new TelegramConfig());
            botClient = new TelegramBotClient(config.token, new HttpClient());
            var me = await botClient.GetMeAsync();
            Console.WriteLine($"Авторизация бота Telegram успешно произошла. Имя бота: {me.Username}");
        }


        public static async void SendMessage(string message)
        {
            try
            {
                await botClient.SendTextMessageAsync(new ChatId(int.Parse(config.chat_id)), message);
            }
            catch { }
        }

    }
}
