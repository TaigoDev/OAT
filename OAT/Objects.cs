using CsvHelper.Configuration.Attributes;

public class Config
{
    public string BaseUrl { get; set; }
    public string MainUrl { get; set; }
    public int bind_port { get; set; }
    public string db_ip { get; set; }
    public int db_port { get; set; }
    public string db_user { get; set; }
    public string db_password { get; set; }
    public string db_name { get; set; }
    public string ldap_IP { get; set; }
    public int ldap_port { get; set; }
    public string ldap_login { get; set; }
    public string ldap_password { get; set; }
    public string ldap_domain { get; set; }
    public string ldap_zone { get; set; }
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
    public NewsFile() { }
}

public class TelegramConfig
{
    public string token { get; set; }
    public string chat_id { get; set; }
}
public class Group
{
    public Group(string name, int curse, List<Week> weeks, List<string> lesson_times)
    {
        this.name = name;
        this.weeks = weeks;
        this.curse = curse;
        this.lesson_times = lesson_times;
    }

    public string name { get; set; }
    public int curse { get; set; }
    public List<Week> weeks { get; set; } = new List<Week>(2);

    public List<string> lesson_times { get; set; }
}

public class Week
{
    public Week(int id, List<Day> days)
    {
        this.id = id;
        this.days = days;
    }

    public int id { get; set; }
    public List<Day> days { get; set; } = new List<Day>(7);
}

public class Day
{
    public Day(string name, List<Lesson> lessons)
    {
        this.name = name;
        this.lessons = lessons;
    }

    public string name { get; set; }
    public List<Lesson> lessons { get; set; } = new List<Lesson>();
}

public class Lesson
{
    public Lesson(int id, List<Subgroup> subgroups)
    {
        this.id = id;
        this.subgroups = subgroups;
    }

    public int id { get; set; }
    public List<Subgroup> subgroups { get; set; } = new List<Subgroup>();
}

public class Subgroup
{
    public Subgroup(int id, string subject, string short_subject, string teacher, string cabinet)
    {
        this.id = id;
        this.subject = subject;
        this.short_subject = short_subject;
        this.teacher = teacher;
        this.cabinet = cabinet;
    }

    public int id { get; set; }
    public string subject { get; set; }
    public string short_subject { get; set; }
    public string teacher { get; set; }
    public string cabinet { get; set; }
}

public class Contract
{
    [Name("NomKontrakt")]
    public string documentId { get; set; }

    [Name("DataKontrakt")]
    public string documentDate { get; set; }
    [Name("FullName")]
    public string studentFullName { get; set; }
    [Name("Gruppa")]
    public string Group { get; set; }
    [Name("Zakazchik")]
    public string FullName { get; set; }
}