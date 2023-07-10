using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Text;
using System.Security.Cryptography;

namespace OAT.function
{
    public static class Utils
    {
        public static string[] GetWords(
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
                return words;   // not so many words found

            // remove last "word" since that contains the rest of the string
            Array.Resize(ref words, words.Length - 1);

            return words;
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
    }
}
