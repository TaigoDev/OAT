using Microsoft.EntityFrameworkCore;
using OAT;
using OAT.Entities.Database;

namespace OMAVIAT.Services.News
{
	public class ProfNewsReader
	{
		public static List<ProfNews> news = [];
		public static IEnumerable<IEnumerable<ProfNews>> pages = [];

		public static async Task init()
		{
			using var connection = new DatabaseContext();
			news = await connection.ProfNews.ToListAsync();
			news = news.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
			news = [.. news.Where(e => e.IsFixed), .. news.Where(e => !e.IsFixed)];
			pages = news.PagesSplit(10);
		}

	}
}
