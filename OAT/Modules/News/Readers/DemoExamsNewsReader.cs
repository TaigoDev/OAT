using MySqlConnector;
using OAT.Entities.Database;
using OAT.Utilities;
using RepoDb;

namespace OAT.Modules.MNews.Readers
{
	public class DemoExamsNewsReader
	{
		public static List<DemoExamNews> news = new();
		public static IEnumerable<IEnumerable<DemoExamNews>> pages = new List<List<DemoExamNews>>();

		public static async Task init()
		{
			using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
			var records = await connection.QueryAllAsync<DemoExamNews>();
			news = records.ToList();
			news.Reverse();
			pages = news.PagesSplit(10);
		}
	}
}
