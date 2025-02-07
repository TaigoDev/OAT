using Microsoft.EntityFrameworkCore;
using OMAVIAT.Entities.Database;

namespace OMAVIAT.Services.News;

public static class MuseumNewsReader
{
	public static List<MuseumEventsNews> MuseumEventsNews = [];
	public static List<MuseumHistoryNews> MuseumHistoryNews = [];
	public static List<MuseumInterestingFactsNews> MuseumInterestingFactsNews = [];
	public static IEnumerable<IEnumerable<MuseumEventsNews>> MuseumEventsNewsPages = [];
	public static IEnumerable<IEnumerable<MuseumHistoryNews>> MuseumHistoryNewsPages = [];
	public static IEnumerable<IEnumerable<MuseumInterestingFactsNews>> MuseumInterestingFactsNewsPages = [];

	public static async Task Init()
	{
		await using var connection = new DatabaseContext();
		MuseumEventsNews = await connection.MuseumEventsNews.ToListAsync();
		MuseumEventsNews = MuseumEventsNews.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
		MuseumEventsNews = [.. MuseumEventsNews.Where(e => e.IsFixed), .. MuseumEventsNews.Where(e => !e.IsFixed)];
		
		MuseumHistoryNews = await connection.MuseumHistoryNews.ToListAsync();
		MuseumHistoryNews = MuseumHistoryNews.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
		MuseumHistoryNews = [.. MuseumHistoryNews.Where(e => e.IsFixed), .. MuseumHistoryNews.Where(e => !e.IsFixed)];

		MuseumInterestingFactsNews = await connection.MuseumInterestingFactsNews.ToListAsync();
		MuseumInterestingFactsNews = MuseumInterestingFactsNews.OrderByDescending(e => DateTime.ParseExact(e.date, "yyyy-MM-dd", null)).ToList();
		MuseumInterestingFactsNews = [.. MuseumInterestingFactsNews.Where(e => e.IsFixed), .. MuseumInterestingFactsNews.Where(e => !e.IsFixed)];

		MuseumEventsNewsPages = MuseumEventsNews.PagesSplit(10);
		MuseumHistoryNewsPages = MuseumHistoryNews.PagesSplit(10);
		MuseumInterestingFactsNewsPages = MuseumInterestingFactsNews.PagesSplit(10);
	}
}