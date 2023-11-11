using MySqlConnector;
using OAT.Utilities;
using RepoDb;
using System.Text;


public class NewsReader
{
    public static IEnumerable<IEnumerable<News>> pages = new List<List<News>>();
    public static List<News> news = new List<News>();

    public static async Task init()
    {
        await ConvertFileToMySQL();
        using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
        var records = await connection.QueryAllAsync<News>();
        news = records.ToList();
        news.OrderBy(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null));
        news.Reverse();
        pages = news.PagesSplit(10);
    }

    [Obsolete]
    public static async Task ConvertFileToMySQL()
    {
        var files = Directory.GetFiles("news", "*.yaml");
        using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
        foreach (string file in files)
        {
            try
            {
                using FileStream fsSource = new FileStream(file, FileMode.Open, FileAccess.Read);
                byte[] buffer = new byte[fsSource.Length];
                await fsSource.ReadAsync(buffer, 0, buffer.Length);

                var id = Path.GetFileNameWithoutExtension(file).ToInt32();
                var news = StringUtils.DeserializeYML<NewsFile>(Encoding.Default.GetString(buffer));
                var _news = new News(id,
                    news.date, news.title, news.text, news.photos);
                var records = await connection.ExistsAsync<News>(e => e.id == id);
                if(await connection.ExistsAsync<News>(e => e.id == id))
                {
                    Logger.Warning($"Новость {id} уже существует в бд!");
                    continue;
                }
                await connection.InsertAsync(_news);
                Logger.InfoWithoutTelegram($"Новость {id} была успешно перенесена в бд");
                fsSource.Close();
                fsSource.Dispose();
                File.Move(file, $"{file}.old");
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

        }

    }


}

