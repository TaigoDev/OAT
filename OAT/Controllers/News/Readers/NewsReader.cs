using MySqlConnector;
using OAT.Entities.Database;
using OAT.Utilities;
using RepoDb;


public class NewsReader
{
	public static IEnumerable<IEnumerable<News>> pages = new List<List<News>>();
	public static List<News> news = new List<News>();

	public static async Task init()
	{
		using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
		var records = await connection.QueryAllAsync<News>();
		news = records.ToList();
		news.OrderBy(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null));
		news.Reverse();
		pages = news.PagesSplit(10);
	}




}

