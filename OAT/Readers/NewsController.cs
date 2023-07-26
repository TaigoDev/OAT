using System.Text;


public class NewsController
{
    public static List<News> News = new List<News>();
    public static List<List<News>> pages = new List<List<News>>();

    public static void init()
    {
        AutoUpdate();
        Console.WriteLine($"OAT.Core.News: We successful load {News.Count} news");
        new Task(() => AutoUpdate());
    }


    protected static async void AutoUpdate()
    {
        while (true)
        {
            Loader();
            await Task.Delay(30000);
        }
    }

    public static async void Loader()
    {
        
        News.Clear();
        string[] files = Directory.GetFiles("news", "*.yaml");
        foreach (string file in files)
        {
            try
            {
                using FileStream fsSource = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fsSource.Length];
                await fsSource.ReadAsync(buffer, 0, buffer.Length);
              
                var news = Utils.DeserializeYML<NewsFile>(Encoding.Default.GetString(buffer));
                News.Add(new News(
                    Path.GetFileNameWithoutExtension(file).ToInt32(),
                    news.text.GetWords(15),
                    news.date,
                    news.title,
                    news.text,
                    news.photos));
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());    
            }
        }

        Console.WriteLine($"OAT.Core.News: We successful load updated news");
        News = News.OrderBy(x => x.id).ToList().Reverse<News>();
        SetupPages();
    }

    protected static void SetupPages()
    {
        pages = new List<List<News>>();
        var newsOnPage = new List<News>();
        for (int i = 0; i < News.Count; i++)
        {
            newsOnPage.Add(News[i]);
            if (newsOnPage.Count == 10 || i + 1 == News.Count)
            {
                pages.Add(newsOnPage);
                newsOnPage = new List<News>();
            }
        }
    }
}

