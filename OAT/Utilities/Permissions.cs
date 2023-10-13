using Microsoft.AspNetCore.Authorization;
using MySqlConnector;
using Newtonsoft.Json;
using RepoDb;
using System.Text;
using static Enums;

namespace OAT.Utilities
{
    public class Permissions
    {

        public static List<Role> GetUserRoles(string username)
        {
            var results = Ldap.SearchByUsername(username);
            var roles = new List<Role>();
            var attribute = Ldap.GetAttributeByTag(results, "memberOf");

            if (attribute is null)
                return new List<Role>();

            var values = attribute!.GetValues(typeof(byte[]));
            for (int id = 0; id < values.Count(); id++)
            {
                var RoleName = Encoding.UTF8.GetString((values[id] as byte[])!).Split(',')[0].Replace("CN=", "");
                var success = Enum.TryParse(RoleName, out Role Role);
                if (!success)
                    continue;
                roles.Add(Role);
            }
            return roles;
        }


        public static bool RightsToBuildingById(string username, string BuildingName)
        {
            var roles = GetUserRoles(username);
            if (roles is null)
                return false;

            if (roles.Any(e => e == Role.www_admin))
                return true;

            foreach (var role in roles)
            {
                var stringArray = role.ToString().Split('_');
                var campusId = stringArray[stringArray.Length - 1].ToInt32();

                if (stringArray[stringArray.Length - 2] != "campus")
                    continue;

                var HavePermission = BuildingName switch
                {
                    "b1" => campusId == 1,
                    "b2" => campusId == 2,
                    "b3" => campusId == 3,
                    "b4" => campusId == 4,
                    _ => false
                };
                if (HavePermission)
                    return true;
            }

            return false;

        }

        public static async Task<bool> RightsToBuilding(string Token, Role role)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var records = await connection.QueryAsync<Tokens>(e => e.Token == Token);
            var record = records.FirstOrDefault();
            if (record is null)
                return false;

            var db_roles = JsonConvert.DeserializeObject<List<Role>>(record.Roles);
            if (db_roles == null)
                return false;

            return db_roles.Any(e => e == role || e == Role.www_admin);
        }


    }



}
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] allowedRoles)
    {
        var allowedRolesAsStrings = GetUnderRoles(allowedRoles.ToList()).ConvertToString();
        Roles = string.Join(",", allowedRolesAsStrings);
    }

    private List<Role> GetUnderRoles(Role role)
    {
        if (!role.ToString().Contains("ALL"))
            return new List<Role>() { role };

        var roles = new List<Role>();
        for (int i = 1; i <= Enums.campus_count; i++)
            roles.Add(Enum.Parse<Role>(role.ToString().Replace("ALL", $"campus_{i}")));
        return roles;
    }

    private List<Role> GetUnderRoles(List<Role> roles)
    {
        var _roles = new List<Role>();
        foreach (var role in roles)
            _roles.AddRange(GetUnderRoles(role));
        return _roles;
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

        www_manager_files_practice_ALL,
        www_manager_files_practice_campus_1,
        www_manager_files_practice_campus_2,
        www_manager_files_practice_campus_3,
        www_manager_files_practice_campus_4,
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