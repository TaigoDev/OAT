using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OAT.Utilities;
using RepoDb;
using RepoDb.Extensions;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using static ProxyController;

public static class Utils
{

    public static void FileDelete(string path)
    {
        if (File.Exists(path))
            File.Delete(path);
    }
    public static string[] GetWordsLocal(
this string input,
int count = -1,
string[] wordDelimiter = null,
StringSplitOptions options = StringSplitOptions.None)
    {
        if (string.IsNullOrEmpty(input)) return new string[] { };

        if (count < 0)
            return input.Split(wordDelimiter, options);

        string[] words = input.Split(wordDelimiter, count + 1, options);
        if (words.Length <= count)
            return words;

        Array.Resize(ref words, words.Length - 1);

        return words;
    }



    public static void CreateDirectoriesWithCurrentPath(params string[] paths)
    {
        foreach (var path in paths)
            CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), path));
    }

    public static void SendEmail(string from, string to, string message)
    {
        var smtpClient = new SmtpClient("smtp-relay.oat.local")
        {
            Port = 25,
            Credentials = new NetworkCredential("service@oat.ru", ""),
            EnableSsl = false,
        };
        MailAddress addressFrom = new MailAddress("service@oat.ru", from);
        MailAddress addressTo = new MailAddress(to);
        MailMessage m = new MailMessage(addressFrom, addressTo);

        m.Body = message;
        m.Subject = "Новый вопрос с сайта oat.ru";
        if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            smtpClient.Send(m);
    }

    public static void CreateDirectory(string path)
    {
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
    }
    public static T SetupConfiguration<T>(string filename, T obj)
    {
        if (!File.Exists($"{filename}"))
            File.WriteAllText($"{filename}", SerializeYML(obj));
        return DeserializeYML<T>(File.ReadAllText($"{filename}"));
    }
    public static void SetCookie(this HttpContext context, string key, string value)
    {
        CookieOptions cookie = new CookieOptions();
        cookie.Expires = DateTime.Now.AddDays(1);
        context.Response.Cookies.Append(key, value, cookie);
    }
    public static void SetCookie(this HttpContext context, string key, string value, int days)
    {
        CookieOptions cookie = new CookieOptions();
        cookie.Expires = DateTime.Now.AddDays(days);
        context.Response.Cookies.Append(key, value, cookie);
    }
    public static IEnumerable<IEnumerable<T>> PagesSplit<T>(this IEnumerable<T> source, int itemsPerSet)
    {
        var sourceList = source as List<T> ?? source.ToList();
        for (var index = 0; index < sourceList.Count; index += itemsPerSet)
            yield return sourceList.Skip(index).Take(itemsPerSet);

    }
    public static string GetSHA256(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
    public static IApplicationBuilder UseNoSniffHeaders(this IApplicationBuilder builder)
    {
        return builder.Use(async (context, next) =>
        {
            context.Response.Headers.Add("X-Content-Type-Options", "sniff");
            await next();
        });
    }
    public static string GetCookie(this HttpContext context, string key) => context.Request.Cookies[key];
    public static void DeleteCookie(this HttpContext context, string key) => context.Response.Cookies.Delete(key);
    public static T toObject<T>(this string json) => JObject.Parse(json).ToObject<T>();
    public static T toObjectJC<T>(this string json) => JsonConvert.DeserializeObject<T>(json);
    public static string toJson(this object json) => JsonConvert.SerializeObject(json);
    public static string SerializeYML(this object obj) => new SerializerBuilder()
.WithNamingConvention(CamelCaseNamingConvention.Instance)
.Build().Serialize(obj);

    public static T DeserializeYML<T>(this string yml) => new DeserializerBuilder()
.WithNamingConvention(CamelCaseNamingConvention.Instance)
.Build().Deserialize<T>(yml);

    public static byte[] ReadFully(Stream input)
    {
        byte[] buffer = new byte[16 * 1024];
        using (MemoryStream ms = new MemoryStream())
        {
            int read;
            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                ms.Write(buffer, 0, read);

            return ms.ToArray();
        }
    }



    public static string sha256_hash(string value)
    {
        StringBuilder Sb = new StringBuilder();

        using (var hash = SHA256.Create())
        {
            Encoding enc = Encoding.UTF8;
            byte[] result = hash.ComputeHash(enc.GetBytes(value));

            foreach (byte b in result)
                Sb.Append(b.ToString("x2"));
        }

        return Sb.ToString();
    }
    public static string RandomString(int length)
    {
        const string chars = "qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890qwertyuiopasdfghjklzxcvbnm1234567890";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[new Random().Next(s.Length)]).ToArray());
    }

    public static async Task<int> getLastId(string table, string parametr = "id")
    {
        int userId = 0;
        using (var connection = new MySqlConnection(GetConnectionString()))
        {
            var obj = await connection.ExecuteQueryAsync<dynamic>($"SELECT MAX({parametr}) AS max FROM `{table}`;");
            if (obj.AsList()[0].max != null)
                userId = obj.AsList()[0].max + 1;
        }
        return userId;
    }
    public static List<T> Reverse<T>(this List<T> list)
    {
        list.Reverse();
        return list;
    }
    public static string GetWords(this string text, int count)
        => string.Join(" ", text.GetWordsLocal(count));


    public static int ToInt32(this string text) =>
        int.Parse(text);

    public static string GetConnectionString() => new MySqlConnectionStringBuilder
    {
        Server = config.db_ip,
        UserID = config.db_user,
        Password = config.db_password,
        Database = config.db_name,
        MaximumPoolSize = 2000u,
        AllowUserVariables = true
    }.ConnectionString;


    public static string GetToken(this ClaimsPrincipal User) =>
        User.Identities.ToList()[0].Claims.ToList()[1].Value;
    public static string GetUsername(this ClaimsPrincipal User) =>
        User.Identities.ToList()[0].Claims.ToList()[0].Value;
    public static bool IsRole(this ClaimsPrincipal User, Enums.Role role)
    {
        var roles = Permissions.GetUserRoles(User.GetUsername());

        if (role.ToString().Contains("ALL"))
            for (int i = 1; i <= 4; i++)
                if (IsRole(User, Enum.Parse<Enums.Role>(role.ToString().Replace("ALL", $"campus_{i}"))))
                    return true;

        foreach (var _role in roles)
            if (_role == role)
                return true;
        return false;
    }
    public static bool IsRole(this ClaimsPrincipal User, params Enums.Role[] roles)
    {
        var userRoles = Permissions.GetUserRoles(User.GetUsername());

        foreach (var role in roles)
            if (User.IsRole(role))
                return true;

        return false;
    }

    public static string UserIP(this HttpContext context) =>
        !string.IsNullOrWhiteSpace(context.Request.Headers["CF-Connecting-IP"]) ?
        context.Request.Headers["CF-Connecting-IP"] : context.Connection.RemoteIpAddress!.ToString();

    public static string Base64Encode(string plainText)
    {
        var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
        return System.Convert.ToBase64String(plainTextBytes);
    }
    public static string Base64Decode(string base64EncodedData)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
        return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
    }

    public static string ToSearchView(this string s) =>
        s.ToLower().Replace(" ", "");

    public static string ConvertStringToHex(String input)
    {
        Byte[] stringBytes = Encoding.UTF8.GetBytes(input);
        StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
        foreach (byte b in stringBytes)
            sbBytes.AppendFormat("{0:X2}", b);

        return sbBytes.ToString();
    }

    public static string ConvertHexToString(String hexInput)
    {
        int numberChars = hexInput.Length;
        byte[] bytes = new byte[numberChars / 2];
        for (int i = 0; i < numberChars; i += 2)
            bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);

        return Encoding.UTF8.GetString(bytes);
    }

    public static Task AutoRepeat(Func<Task> repeat, int minutes)
    {
        new Thread(async () =>
        {
            try
            {
                await repeat.Invoke();
                await Task.Delay(minutes * 60 * 1000);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }).Start();
        return Task.CompletedTask;
    }


}

public class RunModules
{

    public static async void StartModules(params Func<Task>[] modules)
    {
        bool IsError = false;
        foreach (var module in modules)
        {
            try
            {
                await module.Invoke();
            }
            catch (Exception ex)
            {
                IsError = true;
                Logger.Error($"❌ Ошибка загрузки модуля {GetMethodName(module)}. Продолжаю запуск...\nОшибка: {ex}");
            }
        }

        Logger.Info(IsError ? "⚠️ Сайт был запущен, но не все модули были загружены успешно" : "✅ Все модули сайта были успешно загружены");
    }

    private static string GetMethodName(Func<Task> module) =>
        $"{module.Method.DeclaringType!.Name}.{module.Method.Name}";


}

public class Runs<T>
{
    public delegate Task method(T parametr);
    public static async Task InTasks(method method, List<T> parametrs)
    {
        var tasks = new List<Task>();
        foreach (var parametr in parametrs)
            tasks.Add(method.Invoke(parametr));
        await Task.WhenAll(tasks.Where(t => t != null).ToArray());
    }
    public static async Task InTask(method method, List<T> parametrs)
    {
        foreach (var parametr in parametrs)
            await method.Invoke(parametr);
    }
}

public class Repeater
{
    public static void Repeat(Func<Task> method, int time) =>
        new Task(async () =>
        {
            while (true)
            {
                try
                {
                    await method.Invoke();
                    await Task.Delay(time);
                }
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }
        }).Start();
}