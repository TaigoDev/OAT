using MySqlConnector;
using OAT.Entities.Database;
using OAT.Utilities;
using RepoDb;

namespace OAT.Modules.MNews.Readers
{
	public class ProfNewsReader
	{
		public static List<ProfNews> news = new();
		public static IEnumerable<IEnumerable<ProfNews>> pages = new List<List<ProfNews>>();

		public static async Task init()
		{
			using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
			var records = await connection.QueryAllAsync<ProfNews>();
			news = records.ToList();
			news.Reverse();
			pages = news.PagesSplit(10);
		}

	}
}
