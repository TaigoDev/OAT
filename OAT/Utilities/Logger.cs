public class Logger
{


    public static void Info(string message)
            => Write($"[{GetTimeUTC()}]: {message}");

    public static void InfoWithoutTelegram(string message)
            => Write($"[{GetTimeUTC()}]: {message}", true);

    public static void Warning(string message)
        => Write($"[WARNING {GetTimeUTC()}]: {message}");

    public static void Error(string message)
        => Write($"[ERROR {GetTimeUTC()}]: {message}");

    public static void Error(Exception message)
        => Write($"[ERROR {GetTimeUTC()}]: {message}");

    private static async void Write(string message, bool disableTelegram = false, int attempt = 0)
    {
        try
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "Logs", $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log");
            if (File.Exists(path))
                await File.AppendAllTextAsync(path, $"{message}\n");
            else
                await File.WriteAllTextAsync(path, $"{message}\n");
            if (!disableTelegram)
                OAT.Utilities.TelegramBot.SendMessage(message);
            else
                Console.WriteLine(message);
        }
        catch (IOException ex)
        {
            if (attempt > 3)
            {
                Console.WriteLine(ex);
                return;
            }
            await Task.Delay(2000);
            Write(message, disableTelegram, attempt + 1);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private static string GetTimeUTC() =>
        DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss");

}

