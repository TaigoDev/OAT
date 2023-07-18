
using Microsoft.AspNetCore.Authorization;
using static Enums;

namespace Recovery.Tables
{
    public class users
    {
        public users(int id, string FullName, string username, string password, string role)
        {
            this.id = id;
            this.FullName = FullName;
            this.username = username;
            this.password = password;
            this.role = role;
        }
        public users(string FullName, string username, string password, Enums.Role role)
        {
            id = Utils.getLastId("users").GetAwaiter().GetResult();
            this.FullName = FullName;
            this.username = username;
            this.password = password;
            this.role = role.ToString();
        }
        public users(string FullName, string username, string password, string role)
        {
            id = Utils.getLastId("users").GetAwaiter().GetResult();
            this.FullName = FullName;
            this.username = username;
            this.password = password;
            this.role = role;
        }
        public users() { }

        public int id { get; set; }
        public string FullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }

    }
}


public static class Enums
{
    public enum Role { admin, reporter, manager }

    public static string GetRoleList(params Role[] roles)
    {
        var list = string.Empty;
        foreach(var role in roles)
            if(role != roles[roles.Length - 1])
                list += $"{role},";
            else 
                list += role.ToString();
        return list;
    }
}
public class AuthorizeRolesAttribute : AuthorizeAttribute
{
    public AuthorizeRolesAttribute(params Role[] allowedRoles)
    {
        var allowedRolesAsStrings = allowedRoles.Select(x => Enum.GetName(typeof(Role), x));
        Roles = string.Join(",", allowedRolesAsStrings);
    }
}