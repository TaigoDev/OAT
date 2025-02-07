using Microsoft.EntityFrameworkCore;
using OMAVIAT.Entities.Database;

namespace OMAVIAT.Services.News;

public class DemoExamsNewsReader
{
	public static List<DemoExamNews> news = [];
	public static IEnumerable<IEnumerable<DemoExamNews>> pages = [];

	public static async Task Init()
	{
		await using var connection = new DatabaseContext();
		news = await connection.DemoExamNews.ToListAsync();
		news = news.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
		news = [.. news.Where(e => e.IsFixed), .. news.Where(e => !e.IsFixed)];
		pages = news.PagesSplit(10);
	}
}