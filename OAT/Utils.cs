using MySqlConnector;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RepoDb;
using RepoDb.Extensions;
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

    public static void CreateDirectory(params string[] paths)
    {
        foreach (var path in paths)
            CreateDirectory(path);
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


    public static string Username(this ClaimsPrincipal User) =>
        User.Identities.ToList()[0].Claims.ToList()[0].Value;
    public static string Password(this ClaimsPrincipal User) =>
        User.Identities.ToList()[0].Claims.ToList()[1].Value;
    public static bool IsRole(this ClaimsPrincipal User, Enums.Role role) =>
        User.Identities.ToList()[0].Claims.ToList()[2].Value == role.ToString();
}

