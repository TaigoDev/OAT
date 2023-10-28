using OAT.Utilities;
using System.Text;


public class NewsReader
{
    public static List<News> News = new List<News>();
    public static IEnumerable<IEnumerable<News>> pages = new List<List<News>>();

    public static Task init()
    {
        RepeaterUtils.Repeat(Loader, 300000);
        return Task.CompletedTask;
    }

    public static async Task Loader()
    {
        News.Clear();
        var files = Directory.GetFiles("news", "*.yaml");
        foreach (string file in files)
        {
            try
            {
                using FileStream fsSource = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fsSource.Length];
                await fsSource.ReadAsync(buffer, 0, buffer.Length);

                var news = StringUtils.DeserializeYML<NewsFile>(Encoding.Default.GetString(buffer));
                News.Add(new News(
                    Path.GetFileNameWithoutExtension(file).ToInt32(),
                    news.text.GetWords(15),
                    news.date,
                    news.title,
                    news.text,
                    news.photos));
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }
        }

        Logger.InfoWithoutTelegram($"OAT.Core.News: We successful load {News.Count} news");
        News = News.OrderBy(x => x.id).ToList().Reverse<News>();
        pages = News.PagesSplit(10);
    }


}

