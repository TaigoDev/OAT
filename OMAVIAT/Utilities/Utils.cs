using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
using OMAVIAT.Entities.Enums;

public static class Utils
{
	public static T toObject<T>(this string json)
	{
		return JsonConvert.DeserializeObject<T>(json) ?? throw new Exception("Ошибка toObject<T>()");
	}

	public static string toJson(this object json)
	{
		return JsonConvert.SerializeObject(json);
	}

	public static List<T> Reverse<T>(this List<T> list)
	{
		list.Reverse();
		return list;
	}

	public static int ToInt32(this string text)
	{
		return int.Parse(text);
	}

	public static string ToSearchView(this string s)
	{
		return s.ToLower().Replace(" ", "");
	}

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
			EnableSsl = false
		};
		var addressFrom = new MailAddress("service@oat.ru", from);
		var addressTo = new MailAddress(to);
		var m = new MailMessage(addressFrom, addressTo);

		m.Body = message;
		m.Subject = "Новый вопрос с сайта oat.ru";
		if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
			smtpClient.Send(m);
	}

	public static string ConvertToString(this Building? building)
	{
		return building switch
		{
			Building.ul_b_khmelnickogo_281a => "b2",
			Building.pr_kosmicheskij_14a => "b3",
			Building.ul_volkhovstroya_5 => "b4",
			_ => "b1"
		};
	}
}