
public class UrlsContoller
{
    private static List<IPage> pages = new List<IPage>();
    public static void Add(IPage page) => pages.Add(page);

    public static string? Redirect(string path) 
    {
        var cPage = pages.Where(p => $"/{p.current}" == path);
        if (cPage.Count() != 0)
            return cPage.FirstOrDefault()?.current;

        var page = pages.Where(p => $"/{p}" == path || $"/{p}/" == path).FirstOrDefault();
        return page == null ? null : page.current;
    }

    public static void init()
    {
        var urls = new List<IPage>()
        {
            /*
             * Если страница готова, то в первом аргументе прописываем новую относительную ссылку, 
             * а во втором аргументе - относительную ссылку битриксу
             * 
             * Если страница не готова, то в первом аргументе прописываем относительную ссылку битрикса со "/" в конце!,
             * а во втором аргументе - новую относительную ссылку
             * Во всех предыдущих случаях на других страницах мы указываем новую относительную ссылку
             * 
             * Если файл страницы вообще отсутствует, то на других страницах, ссылающихся на данную, 
             * мы указываем относительную ссылку на битрикс. В файле UrlsController ничего не указываем.
             */ 
            // main links
            new IPage("allnews", "novosti"),
            new IPage("applicant", "abitur"),
            // organization information
            new IPage("organizationInformation/AccessibleEnvironment", "sveden/ovz"),
            new IPage("organizationInformation/CommonIntelligence", "sveden/common"),
            new IPage("organizationInformation/Jobs", "sveden/vacant"),
            new IPage("organizationInformation/Vacancies", "sveden/vacancy"),

            // limitation
            //new IPage("sveden/ovz/", "organizationInformation/AccessibleEnvironment"),
        };
        pages = urls;
    }
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
