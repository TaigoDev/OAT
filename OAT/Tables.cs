using System;
using YamlDotNet.Core.Tokens;

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
            this.role = Enum.Parse<Enums.Role>(role);
        }

        public users(string FullName, string username, string password, Enums.Role role)
        {
            this.id = id;
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
        public Enums.Role role { get; set; }

    }
}


public static class Enums
{
    public enum Role { admin, reporter, manager }

}