public class Config
{
    public string BaseUrl { get; set; }

    public Config(string BaseUrl, string MainUrl, int bind_port)
    {
        this.BaseUrl = BaseUrl;
        this.MainUrl = MainUrl;
        this.bind_port = bind_port;
    }
    public Config() { }
    public string MainUrl { get; set; }
    public int bind_port { get; set; }
}
public class IPage
{
    public string current { get; set; }
    public string map { get; set; }

    public IPage(string map, string current)
    {
        this.current = current;
        this.map = map;
    }

}

public class News
{
    public News(int id, string short_description, string date, string title, string text, List<string> photos)
    {
        this.id = id;
        this.short_description = short_description;
        this.date = date;
        this.title = title;
        this.text = text;
        this.photos = photos;
    }

    public int id { get; set; }
    public string short_description { get; set; }
    public string date { get; set; } = "01.01.2001";
    public string title { get; set; } = "Новая статья на сайте OAT";
    public string text { get; set; } = "Текст новой статьи на сайте OAT";
    public List<string> photos { get; set; } = new List<string>();
}

public class NewsFile
{
    public string date { get; set; } = "01.01.2001";
    public string title { get; set; } = "Новая статья на сайте OAT";
    public string text { get; set; } = "Текст новой статьи на сайте OAT";
    public List<string> photos { get; set; } = new List<string>();
    public NewsFile(string date, string title, string text, List<string> photos)
    {
        this.date = date;
        this.title = title;
        this.text = text;
        this.photos = photos;
    }

}