using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

public static class Utils
{

    public static T toObject<T>(this string json) => 
        JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Ошибка toObject<T>()");

    public static string toJson(this object json) => 
        JsonConvert.SerializeObject(json);

    public static List<T> Reverse<T>(this List<T> list)
    {
        list.Reverse();
        return list;
    }

    public static int ToInt32(this string text) =>
        int.Parse(text);

    public static string ToSearchView(this string s) =>
        s.ToLower().Replace(" ", "");

    public static IEnumerable<IEnumerable<T>> PagesSplit<T>(this IEnumerable<T> source, int itemsPerSet)
    {
        var sourceList = source as List<T> ?? source.ToList();
        for (var index = 0; index < sourceList.Count; index += itemsPerSet)
            yield return sourceList.Skip(index).Take(itemsPerSet);

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
}




