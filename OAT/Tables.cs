using OAT.Utilities;

[MysqlTable]
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

[MysqlTable]
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
        id = DataBaseUtils.getLastId("ProfNews").GetAwaiter().GetResult();
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

[MysqlTable]
public class Tokens
{

    public Tokens() { }

    public Tokens(int id, string username, string token, string issued)
    {
        this.id = id;
        this.username = username;
        Token = token;
        this.issued = issued;
    }
    public Tokens(string username, string token, string issued, string Roles)
    {
        id = DataBaseUtils.getLastId("Tokens").GetAwaiter().GetResult(); ;
        this.username = username;
        Token = token;
        this.issued = issued;
        this.Roles = Roles;
    }
    public int id { get; set; }
    public string username { get; set; }
    public string Token { get; set; }
    public string Roles { get; set; }
    public string issued { get; set; }
}



[MysqlTable]
public class News
{
    public News(int id, string date, string title, string description, string photos)
    {
        this.id = id;
        this.date = date;
        this.title = title;
        this.description = description;
        this.photos = photos;
    }

    public News(int id, string date, string title, string description, List<string> photos)
    {
        this.id = id;
        this.date = date;
        this.title = title;
        this.description = description;
        this.photos = photos.toJson();
    }

    public News(string date, string title, string description, List<string> photos)
    {
        id = DataBaseUtils.getLastId("News").GetAwaiter().GetResult();
        this.date = date;
        this.title = title;
        this.description = description;
        this.photos = photos.toJson();
    }
    public News() { }
    public int id { get; set; }
    public string date { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public string photos { get; set; }

}