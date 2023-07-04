
public class UrlsContoller
{
    private static List<IPage> pages = new List<IPage>();
    public static void Add(IPage page) => pages.Add(page);

    public static string? Redirect(string path) 
    {
        var cPage = pages.Where(p => $"/{p.current}" == path);
        if (cPage.Count() != 0)
            return cPage.FirstOrDefault()?.current;

        var page = pages.Where(p => p.maps.Where(m => $"/{m}" == path || $"/{m}/" == path).Count() == 0 ? false : true).FirstOrDefault();
        return page == null ? null : page.current;
    }

    public static void init()
    {
        var urls = new List<IPage>()
        {
            new IPage("allnews", "novosti"),
            new IPage("applicant", "abitur"),
            new IPage("sveden/ovz/", "organizationInformation/AccessibleEnvironment"),
        };
        pages = urls;
    }
}

public class IPage
{
    public string current { get; set; }
    public string[] maps { get; set; }

    public IPage(string current, params string[] maps)    {
        this.current = current;
        this.maps = maps;
    }

}
