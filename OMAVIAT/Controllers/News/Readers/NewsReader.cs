using Microsoft.EntityFrameworkCore;
using OAT;
using OAT.Entities.Database;


public class NewsReader
{
	public static IEnumerable<IEnumerable<News>> pages = new List<List<News>>();
	public static List<News> news = [];

	public static async Task init()
	{
		using var connection = new DatabaseContext();
		news = await connection.News.ToListAsync();
		news = news.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
		news = [.. news.Where(e => e.IsFixed), .. news.Where(e => !e.IsFixed)];
		pages = news.PagesSplit(10);
	}




}

