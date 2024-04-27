using Microsoft.EntityFrameworkCore;
using OAT.Entities.Database;

namespace OAT.Controllers.MNews.Readers
{
	public class ProfNewsReader
	{
		public static List<ProfNews> news = [];
		public static IEnumerable<IEnumerable<ProfNews>> pages = [];

		public static async Task init()
		{
			using var connection = new DatabaseContext();
			var records = await connection.ProfNews.ToListAsync();
			news = [.. records];
			news = [.. news.OrderBy(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null))];
			news.Reverse();
			pages = news.PagesSplit(10);
		}

	}
}
