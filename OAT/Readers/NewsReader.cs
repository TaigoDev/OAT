using Recovery.Tables;
using System.Text;


public class NewsReader
{
    public static List<News> News = new List<News>();
    public static IEnumerable<IEnumerable<News>> pages = new List<List<News>>();

    public static void init()
    {
        AutoUpdate();
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
        try
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
                catch (Exception ex)
                {
                    Logger.Error(ex.ToString());
                }
            }

            Console.WriteLine($"OAT.Core.News: We successful load {News.Count} news");
            News = News.OrderBy(x => x.id).ToList().Reverse<News>();
            pages = News.PagesSplit(10);
        }
        catch (Exception ex)
        {
            Logger.Error(ex.ToString());
        }
    }


}

