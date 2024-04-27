using Microsoft.EntityFrameworkCore;
using OAT.Entities.Database;

namespace OAT.Controllers.MNews.Readers
{
	public class DemoExamsNewsReader
	{
		public static List<DemoExamNews> news = [];
		public static IEnumerable<IEnumerable<DemoExamNews>> pages = [];

		public static async Task init()
		{
			using var connection = new DatabaseContext();
			news = await connection.DemoExamNews.ToListAsync();
			news = [.. news.OrderBy(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null))];
			news.Reverse();
			pages = news.PagesSplit(10);
		}
	}
}
