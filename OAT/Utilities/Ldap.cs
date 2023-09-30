using System.DirectoryServices.Protocols;
using System.Net;
using System.Text;

namespace OAT.Utilities
{
    public class Ldap
    {

        public static bool Login(string username, string password, string IP, bool WithError = true)
        {
            try
            {
                var authType = OperatingSystem.IsWindows() ? AuthType.Negotiate : AuthType.Basic;
                username = OperatingSystem.IsWindows() ? username : $"{ProxyController.config.ldap_domain}\\{username}";
                var conn = new LdapConnection(new LdapDirectoryIdentifier(ProxyController.config.ldap_IP, ProxyController.config.ldap_port), new NetworkCredential(username, password), authType);
                conn.Bind();
                return true;
            }
            catch (Exception ex)
            {
                if (WithError)
                    TelegramBot.SendMessage($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
                        $"L: {username}\n" +
                        $"IP-адрес отправителя: {IP}", ex);
                return false;
            }
        }

        public static string? GetStudentGroup(string username)
        {
            var response = SearchByUsername(username);
            var attributes = GetValuesAttributeByTag(response, "memberOf");

            if (attributes is null || !attributes.Any(e => e.Contains("CN=") && e.Contains("Группа -")))
                return null;

            var groupName = attributes.FirstOrDefault(e => e.Contains("OU="));
            if (groupName is null)
                return null;

            return groupName.Replace("OU=", "");      
        }

        public static string? GetFullName(string username)
        {
            var response = SearchByUsername(username);
            var attributes = GetValuesAttributeByTag(response, "cn");
            return attributes is not null ? attributes.First().Replace("{", "").Replace("}", "") : null;    
        }

        public static SearchResponse SearchByUsername(string username)
        {
            using var ldap = new LdapConnection(new LdapDirectoryIdentifier(ProxyController.config.ldap_IP, ProxyController.config.ldap_port));

            ldap.SessionOptions.ProtocolVersion = 3;
            ldap.AuthType = AuthType.Basic;
            ldap.Bind(new NetworkCredential(ProxyController.config.ldap_login, ProxyController.config.ldap_password));

            var search = new SearchRequest
            {
                DistinguishedName = $"DC={ProxyController.config.ldap_domain},DC={ProxyController.config.ldap_zone}",
                Filter = $"(&(samaccountname={username}))",
                Scope = SearchScope.Subtree
            };

            search.Attributes.Add(null);
            return (SearchResponse)ldap.SendRequest(search);
        }

        public static DirectoryAttribute? GetAttributeByTag(SearchResponse results, string tag) =>
            results.Entries.Count is 0 ? null : 
            results.Entries[0].Attributes.Values.Cast<DirectoryAttribute>().FirstOrDefault(e => e.Name == tag);
        
        public static List<string>? GetValuesAttributeByTag(SearchResponse results, string tag)
        {
            var attribute = GetAttributeByTag(results, tag);
            if (attribute is null)
                return null;

            var values = attribute!.GetValues(typeof(byte[]));
            var StringValues = new List<string>();

            for (int id = 0; id < values.Count(); id++)
                StringValues.AddRange(Encoding.UTF8.GetString((values[id] as byte[])!).Split(','));

            return StringValues;
        }

    }
}
