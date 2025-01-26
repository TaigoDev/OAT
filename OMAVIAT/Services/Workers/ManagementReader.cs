using Ganss.Excel;
using NLog;
using OMAVIAT.Entities.Models;

namespace OMAVIAT.Services.Workers;

public static class ManagementReader
{
	public static List<ManagementModel> Managements = [];
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task Init()
	{
		var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "workers", "management.xlsx");
		if (!File.Exists(file))
		{
			Logger.Warn("[WorkersReader]: Не удалось найти файл с работниками (management.xlsx)");
			return;
		}

		await using var stream = File.Open(file, FileMode.Open, FileAccess.Read);

		var excel = new ExcelMapper();

		var reader = await excel.FetchAsync<ManagementModel>(stream);
		Managements = reader.ToList();
	}
}