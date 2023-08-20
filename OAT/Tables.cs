using Microsoft.AspNetCore.Authorization;
using OAT.Utilities;

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
        public users(string FullName, string username, string password, Enums.Role role, Enums.Building building)
        {
            id = Utils.getLastId("users").GetAwaiter().GetResult();
            this.FullName = FullName;
            this.username = username;
            this.password = password;
            this.role = role.ToString();
            this.building = building.ToString();
        }
        public users(string FullName, string username, string password, string role, string building)
        {
            id = Utils.getLastId("users").GetAwaiter().GetResult();
            this.FullName = FullName;
            this.username = username;
            this.password = password;
            this.role = role;
            this.building = building;
        }

        public users() { }

        public int id { get; set; }
        public string FullName { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public string role { get; set; }
        public string building { get; set; }
    }

    public class Teachers
    {
        public Teachers(int id, string FullName, string email, string phone, string base64_profile, string image_url)
        {
            this.id = id;
            this.FullName = FullName;
            this.email = email;
            this.phone = phone;
            this.base64_profile = base64_profile;
            this.image_url = image_url;
        }
        public Teachers() { }
        public int id { get; set; }
        public string FullName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string base64_profile { get; set; }
        public string image_url { get; set; }
    }

    public class ProfNews
    {
        public ProfNews(int id, string date, string title, string description, string short_description, string photos)
        {
            this.id = id;
            this.date = date;
            this.title = title;
            this.description = description;
            this.short_description = short_description;
            this.photos = photos;
        }

        public ProfNews(string date, string title, string description, string short_description, List<string> photos)
        {
            id = Utils.getLastId("ProfNews").GetAwaiter().GetResult();
            this.date = date;
            this.title = title;
            this.description = description;
            this.short_description = short_description;
            this.photos = photos.toJson();
        }
        public ProfNews() { }
        public int id { get; set; }
        public string date { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string short_description { get; set; }
        public string photos { get; set; }

    }
}

