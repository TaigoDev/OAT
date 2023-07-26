using HtmlAgilityPack;
using MySqlConnector;
using Recovery.Tables;
using RepoDb;
using System.Reflection;
using System.Text.RegularExpressions;
using static Utils;

public interface ICommand
{
    public string name { get; set; }
    public Regex pattern { get; set; }
    public string description { get; set; }
    public void Execute(string[] args);
}


public class Help : ICommand
{
    public string name { get; set; } = "help";
    public Regex pattern { get; set; } = new Regex(@"^/\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string description { get; set; } = "Показать это меню";

    public void Execute(string[] args)
    {
        try
        {
            Console.WriteLine("\nВсе команды:");
            foreach (var cmds in CommandsController.commands)
                Console.WriteLine($"/{cmds.name} - {cmds.description};");
        }
        catch { }
    }
}


public class GetHashVersion : ICommand
{
    public string name { get; set; } = "uuid";
    public Regex pattern { get; set; } = new Regex(@"^/\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string description { get; set; } = "получить uuid сборки";

    public void Execute(string[] args) => Console.WriteLine($"Build UUID: {Utils.sha256_hash(new FileInfo(Assembly.GetExecutingAssembly().Location).LastWriteTime.ToString())}");
}

public class Import : ICommand
{
    public string name { get; set; } = "import";
    public Regex pattern { get; set; } = new Regex(@"^/\w*", RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string description { get; set; } = "используйте /import teaching-staff -y, чтобы импортировать весь персонал";
    private string url;
    public void Execute(string[] args)
    {
        try
        {
            if (args.Length == 2)
            {
                if (args[1] == "teaching-staff")
                    Console.WriteLine("Подтвердите операцию добавив -y");
                else
                    Console.WriteLine("Не найден тип импорта");

            }
            else if (args.Length <= 1)
            {
                Console.WriteLine("Не найден тип импорта");
            }
            else if (args[2] != "-y")
                Console.WriteLine("Подтвердите операцию добавив -y");
            else
            {
                url = args[3];
                new Task(() => ImportTeachers()).Start();
            }
        }
        catch(Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }

    private async void ImportTeachers()
    {
        Logger.Info("Начинаю импорт преподавателей");
        var ids = new List<string>();
        for(int i = 0; i <= 999; i++)
            ids.Add(GetId(i));
        await Runs<string>.InTask(Load, ids);
        Logger.Info("Импорт завершен");
    } 

    private string GetId(int i)
    {
        if (i < 10) return $"00{i}";
        else if (i >= 10 && i < 100) return $"0{i}";
        else return i.ToString();
    }

    private async Task Load(string id)
    {
        try
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(await DownloadHtml(id));

            var container = doc.DocumentNode.Descendants(0).Where(n => n.HasClass("main__content")).FirstOrDefault();
            if (container == null)
            {
                Logger.Info($"Ошибка импорта профиля {id}");
                return;
            }

            if (container.InnerText.Contains("Элемент не найден!"))
                return;
            var profile = container.Descendants(0).Where(n => n.HasClass("profile_photo_main")).First().ChildNodes;
            var data = container.Descendants(0).Where(n => n.HasClass("profile_text_main")).First();
            var contact = data.ChildNodes.FindFirst("p").InnerText.Split("\n");
            var field_profile = data.ChildNodes.Where(n => n.HasClass("field_profile"));

            var FullName = profile.FindFirst("h3").InnerText;
            var photo_url = await SaveImage(profile.FindFirst("div").Attributes["href"].Value, FullName);
            var email = contact[1].Replace("E-mail: ", "");
            var telephone = contact[1].Replace("Телефон: ", "");
            var job = profile.FindFirst("p").InnerText;

            data.RemoveChild(data.ChildNodes.FindFirst("p"));
            var info = data.InnerText;

            using var connection = new MySqlConnection(Utils.GetConnectionString());

            if((await connection.QueryAsync<Teachers>(n => n.id == Convert.ToInt32(id))).Any())
            {
                OAT.Utilities.Telegram.SendMessage($"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: Пропуск {id} уже есть в бд");
                return;
            }
            await connection.InsertAsync(new Teachers(int.Parse(id), FullName,
                email, telephone, Base64Encode(info), photo_url)); 

            OAT.Utilities.Telegram.SendMessage($"[{DateTime.UtcNow.ToString("dd.MM.yyyy mm:HH:ss")}]: ✅ Импорт {FullName} успешно завершен\n" +
                $"photo_url: {photo_url}\n" +
                $"email: {email}\n" +
                $"telephone: {telephone}\n" +
                $"job: {job}\n " +
                $"info: {Utils.sha256_hash(info)}");
        }
        catch(Exception ex)
        {
            Logger.Error($"Ошибка импорта {id}: {ex}");
        }
    }

    private async Task<string> DownloadHtml(string id, int i = 0)
    {
        try
        {
            var httpClient = new HttpClient();
            var html = await httpClient.GetStringAsync($"{url}/sveden/employees/{id}/");
            return html;
        }
        catch(Exception ex )
        {
            if (i == 10)
                Logger.Error($"Ошибка скачивания страницы: {url}/sveden/employees/{id}/:\n{ex}");

            await Task.Delay(10000);
            return await DownloadHtml(id, i + 1);
        }
    }

    private async Task<string> SaveImage(string url, string FullName)
    {
        if (string.IsNullOrWhiteSpace(url))
            return "";
        using var client = new HttpClient();
        client.BaseAddress = new Uri(this.url);
        var steam = await client.GetStreamAsync(url);
        using FileStream outputFileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "static", "teachers", $"{sha256_hash(FullName)}.png"), FileMode.Create);
        await steam.CopyToAsync(outputFileStream);
        return $"static/teachers/{sha256_hash(FullName)}.png";
    }
}

