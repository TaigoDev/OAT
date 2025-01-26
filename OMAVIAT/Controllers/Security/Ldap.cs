using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;
using OMAVIAT.Utilities.Telegram;

namespace OMAVIAT.Controllers.Security;

public class Ldap
{
	public static bool Login(string username, string password, string ip, bool withError = true)
	{
		if (!Configurator.Config.IsProduction)
			return true;

		try
		{
			if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
				return false;

			var authType = OperatingSystem.IsWindows() ? AuthType.Negotiate : AuthType.Basic;
			username = OperatingSystem.IsWindows() ? username : $"{Configurator.Config.Ldap.Domain}\\{username}";
			var conn = new LdapConnection(new LdapDirectoryIdentifier(Configurator.Config.Ldap.Address,
				Configurator.Config.Ldap.Port), new NetworkCredential(username, password), authType);
			conn.Bind();
			return true;
		}
		catch (Exception ex)
		{
			if (withError)
				TelegramBot.SendMessage($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
				                        $"L: {username}\n" +
				                        $"IP-адрес отправителя: {ip}." +
				                        $"Ошибка: {ex}", ex);
			return false;
		}
	}

	public static string? GetStudentGroup(string username)
	{
		var response = SearchByUsername(username);
		var attributes = GetValuesAttributeByTag(response, "memberOf");
		var memberOf = attributes?.FirstOrDefault(e => e.Contains("CN=") && e.Contains("Группа -"));
		return memberOf?.Replace("CN=", "").Replace("Группа - ", "");
	}

	public static string? GetFullName(string username)
	{
		if (!Configurator.Config.IsProduction)
			return "Admin Admin Admin";
		var response = SearchByUsername(username);
		var attributes = GetValuesAttributeByTag(response, "cn");
		return attributes?.First().Replace("{", "").Replace("}", "");
	}

	public static SearchResponse SearchByUsername(string username)
	{
		using var ldap = new LdapConnection(new LdapDirectoryIdentifier(Configurator.Config.Ldap.Address,
			Configurator.Config.Ldap.Port));

		ldap.SessionOptions.ProtocolVersion = 3;
		ldap.AuthType = AuthType.Basic;
		ldap.Bind(new NetworkCredential(Configurator.Config.Ldap.Username, Configurator.Config.Ldap.Password));

		var search = new SearchRequest
		{
			DistinguishedName = $"DC={Configurator.Config.Ldap.Domain},DC={Configurator.Config.Ldap.Zone}",
			Filter = $"(&(samaccountname={username}))",
			Scope = SearchScope.Subtree
		};

		search.Attributes.Add(null);
		return (SearchResponse)ldap.SendRequest(search);
	}

	public static DirectoryAttribute? GetAttributeByTag(SearchResponse results, string tag)
	{
		return results.Entries.Count is 0
			? null
			: results.Entries[0].Attributes.Values.Cast<DirectoryAttribute>().FirstOrDefault(e => e.Name == tag);
	}

	public static List<string>? GetValuesAttributeByTag(SearchResponse results, string tag)
	{
		var attribute = GetAttributeByTag(results, tag);
		if (attribute is null)
			return null;

		var values = attribute.GetValues(typeof(byte[]));
		var stringValues = new List<string>();

		foreach (var t in values)
			stringValues.AddRange(Encoding.UTF8.GetString((t as byte[])!).Split(','));

		return stringValues;
	}
}