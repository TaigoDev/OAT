using Microsoft.AspNetCore.Authorization;
using OAT.Utilities;
using System.DirectoryServices.Protocols;
using System.Text;
using static Enums;

namespace OAT.Utilities
{
    public class Permissions
    {



        public static List<Role> GetUserRoles(string username)
        {

            using var ldap = new LdapConnection(new LdapDirectoryIdentifier(ProxyController.config.ldap_IP, ProxyController.config.ldap_port));

            ldap.SessionOptions.ProtocolVersion = 3;
            ldap.AuthType = AuthType.Basic;
            ldap.Bind(new System.Net.NetworkCredential(ProxyController.config.ldap_login, ProxyController.config.ldap_password));

            var search = new SearchRequest
            {
                DistinguishedName = $"DC={ProxyController.config.ldap_domain},DC={ProxyController.config.ldap_zone}",
                Filter = $"(&(samaccountname={username}))",
                Scope = SearchScope.Subtree
            };

            search.Attributes.Add(null);
            var results = (SearchResponse)ldap.SendRequest(search);
            if (results.Entries.Count == 0)
                return new List<Role>();

            var roles = new List<Role>();
            var attribute = results.Entries[0].Attributes.Values.Cast<DirectoryAttribute>().FirstOrDefault(e => e.Name == "memberOf");
            if (attribute is null)
                return new List<Role>();

            var values = attribute!.GetValues(typeof(byte[]));
            for (int id = 0; id < values.Count(); id++)
            {
                var RoleName = Encoding.UTF8.GetString((values[id] as byte[])!).Split(',')[0].Replace("CN=", "");
                var Role = Enums.Role.www_reporter_prof_news;
                var success = Enum.TryParse(RoleName, out Role);
                if (!success)
                    continue;
                roles.Add(Role);
            }
           return roles;
        }


        public static List<Role> GetUnderRoles(Role role)
        {
            if (!role.ToString().Contains("ALL"))
                return new List<Role>() { role };

            var roles = new List<Role>();
            for (int i = 1; i <= Enums.campus_count; i++)
                roles.Add(Enum.Parse<Role>(role.ToString().Replace("ALL", $"campus_{i}")));
            return roles;
        }

        public static List<Role> GetUnderRoles(List<Role> roles)
        {
            var _roles = new List<Role>();
            foreach (var role in roles)
                _roles.AddRange(GetUnderRoles(role));
            return _roles;
        }
        public static bool HavePermissionСampus(string username, string BuildingName)
        {
            var roles = GetUserRoles(username);
            if (roles is null)
                return false;

            if (roles.Any(e => e == Role.www_admin))
                return true;

            foreach (var role in roles)
            {
                var stringArray = role.ToString().Split('_');
                if (stringArray[stringArray.Length - 2] != "campus")
                    continue;

                var id = stringArray[stringArray.Length - 1].ToInt32();
                var HavePermission = BuildingName switch
                {
                    "ul_lenina_24" => id == 1,
                    "ul_b_khmelnickogo_281a" => id == 2,
                    "pr_kosmicheskij_14a" => id == 3,
                    "ul_volkhovstroya_5" => id == 4,
                    _ => false
                };
                if (HavePermission)
                    return true;
            }

            return false;

        }

        public static bool HavePermissionСampusById(string username, string BuildingName)
        {
            var roles = GetUserRoles(username);
            if (roles is null)
                return false;

            if (roles.Any(e => e == Role.www_admin))
                return true;

            foreach (var role in roles)
            {
                var stringArray = role.ToString().Split('_');
                if (stringArray[stringArray.Length - 2] != "campus")
                    continue;

                var id = stringArray[stringArray.Length - 1].ToInt32();
                var HavePermission = BuildingName switch
                {
                    "b1" => id == 1,
                    "b2" => id == 2,
                    "b3" => id == 3,
                    "b4" => id == 4,
                    _ => false
                };
                if (HavePermission)
                    return true;
            }

            return false;

        }

        public static bool HavePermissionСampus(string username, Building BuildingName) =>
            HavePermissionСampus(username, BuildingName.ToString());

    }



}
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] allowedRoles)
    {
        var allowedRolesAsStrings = Permissions.GetUnderRoles(allowedRoles.ToList()).ConvertToString();
        Roles = string.Join(",", allowedRolesAsStrings);
    }
}
public static class Enums
{

    /*
    admin - все ниже, а также управление пользователями и MySql
    reporter - новости, новости професионалитета 
    schedule_manager - расписание, изменение расписания, документы сессии
     */
    public static readonly int campus_count = 4;
    public enum Role
    {
        www_admin,
        www_reporter_news,
        www_reporter_prof_news,

        www_manager_schedule_ALL,
        www_manager_schedule_campus_1,
        www_manager_schedule_campus_2,
        www_manager_schedule_campus_3,
        www_manager_schedule_campus_4,

        www_manager_changes_ALL,
        www_manager_changes_campus_1,
        www_manager_changes_campus_2,
        www_manager_changes_campus_3,
        www_manager_changes_campus_4,

        www_manager_files_sessions_ALL,
        www_manager_files_sessions_campus_1,
        www_manager_files_sessions_campus_2,
        www_manager_files_sessions_campus_3,
        www_manager_files_sessions_campus_4,
    }
    public static List<string> ConvertToString(this List<Role> roles)
    {
        var _roles = new List<string>();
        foreach (var role in roles)
            _roles.Add(role.ToString());
        return _roles;
    }
    public enum Building
    {
        all,
        ul_lenina_24,
        ul_b_khmelnickogo_281a,
        pr_kosmicheskij_14a,
        ul_volkhovstroya_5
    }

    public enum AuthResult
    {
        success,
        token_expired,
        fail
    }
}