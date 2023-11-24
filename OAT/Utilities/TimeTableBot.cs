using static Enums;

namespace OAT.Utilities
{
    public class TimeTableBot
    {

        public static TimeTableBotConfig config = new();

        public static async Task init() =>
            config = await FileUtils.SetupConfiguration("timetable_bot.yml", new TimeTableBotConfig());

        public static async Task onChangesInSchedule(string building, string xlsx)
        {
            try
            {
                var client = new HttpClient();

                await using var stream = File.OpenRead(xlsx);
                using var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{config.url}/api/alerts/changes/schedule/{building}");
                using var content = new MultipartFormDataContent
                {
                    { new StringContent(config.token), "token" },
                    { new StreamContent(stream), "file", $"{building}.xlsx" }
                };

                request.Content = content;
                await client.SendAsync(request);
            }
            catch(Exception ex)
            {
                Logger.Error($"Ошибка при отправке запроса к боту расписания. Попробую ещё раз через 25 минут...\n{ex}");
                await Task.Delay(new TimeSpan(0, 25, 0));
                Runs.InThread(async () => await onChangesInSchedule(building, xlsx));
            }
        }

        public static async Task onChangeMainSchedule(string building, string xml)
        {

            try
            {
                var client = new HttpClient();
                await using var stream = File.OpenRead(xml);
                using var request = new HttpRequestMessage(HttpMethod.Post,
                    $"{config.url}/api/alerts/schedule/{building}");
                using var content = new MultipartFormDataContent
                {
                    { new StringContent(config.token), "token" },
                    { new StreamContent(stream), "file", $"{building}.xml" }
                };

                request.Content = content;
                await client.SendAsync(request);
            }
            catch (Exception ex)
            {
                Logger.Error($"Ошибка при отправке запроса к боту расписания. Попробую ещё раз через 25 минут...\n{ex}");
            }
        }
    }
}
