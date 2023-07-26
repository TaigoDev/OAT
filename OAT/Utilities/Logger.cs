public class Logger
{

    public static string path = Path.Combine(Directory.GetCurrentDirectory(), "Logs");
    public static string path_PreventedAttempts = Path.Combine(Directory.GetCurrentDirectory(), "Logs", "PreventedAttempts");

    public static void Info(string message)
    {
        OAT.Utilities.Telegram.SendMessage($"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}");
        if (HasFilePreventedAttempts())
            File.AppendAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
        else
            File.WriteAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
    }

    public static void InfoInAttempts(string message)
    {
        OAT.Utilities.Telegram.SendMessage($"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}");
        if (HasFile())
            File.AppendAllText(
                Path.Combine(path_PreventedAttempts, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
        else
            File.WriteAllText(
                Path.Combine(path_PreventedAttempts, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
    }

    public static void Warning(string message)
    {
        OAT.Utilities.Telegram.SendMessage($"[WARNING {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}");
        if (HasFile())
            File.AppendAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[WARNING {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
        else
            File.WriteAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[WARNING {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
    }

    public static void Error(string message)
    {
        OAT.Utilities.Telegram.SendMessage($"[ERROR {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}");
        if (HasFile())
            File.AppendAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[ERROR {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
        else
            File.WriteAllText(
                Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log"),
                $"[ERROR {DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: {message}\n");
    }

    private static bool HasFile() =>
        File.Exists(Path.Combine(path, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log")) ? true : false;

    private static bool HasFilePreventedAttempts() =>
        File.Exists(Path.Combine(path_PreventedAttempts, $"{DateTime.UtcNow.ToString("dd-MM-yyyy")}.log")) ? true : false;

}

