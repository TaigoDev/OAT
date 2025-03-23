using Ganss.Excel;
using NLog;
using OMAVIAT.Entities.Models;
using RepoDb.Extensions;

namespace OMAVIAT.Services.Workers;

public static class WorkersReader
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	public static List<List<Worker>> Administration { get; set; } = [];
	public static IEnumerable<IEnumerable<Worker>> Workers { get; set; } = new List<List<Worker>>();
	public static List<Worker> AllWorkers { get; set; } = [];

	public static async Task Init()
	{
		var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "workers", "workers.xlsx");
		if (!File.Exists(file))
		{
			Logger.Warn("[WorkersReader]: Не удалось найти файл с работниками (workers.xlsx)");
			return;
		}

		await using var stream = File.Open(file, FileMode.Open, FileAccess.Read);

		var excel = new ExcelMapper
		{
			HeaderRow = true,
			HeaderRowNumber = 7,
			MinRowNumber = 8,
			CreateMissingHeaders = true
		};

		var reader = await excel.FetchAsync<Worker>(stream);
		var workers = reader.ToList();
		var ignoredPost = new List<string>
		{
			"Уборщик служебных помещений",
			"Рабочий по комплексному обслуживанию и ремонту зданий",
			"Слесарь-сантехник",
			"Сторож (вахтёр)",
			"Дежурный по общежитию",
			"Электромонтер по ремонту и обслуживанию электрооборудования",
			"Инженер-электроник (электроник)",
			"Кассир",
			"Плотник",
			"Воспитатель",
			"Гардеробщик",
			"Дворник",
			"Инженер",
			"Кладовщик",
			"Маляр",
			"Механик",
			"Плотник",
			"Слесарь-ремонтник",
			"Табельщик",
			"Фельдшер",
			"Водитель автомобиля",
			"Лаборант",
			"Старший лаборант",
			"Старший мастер",
			"Уборщик производственных помещений",
			"Инженер-программист (программист)",
			"Прокин Никита Григорьевич",
			"Елкин Александр Андреевич",
			"Елкин Иван Андреевич",
			"Принцев Денис Игоревич"
		};

		workers.RemoveAll(e => ignoredPost.Contains(e.Post) || ignoredPost.Contains(e.FullName));
		AllWorkers.AddRange(workers);
		AdministrationByPost(ref workers, e => e is "Директор");
		AdministrationByPost(ref workers, e => e.Contains("Заместитель директора") && !e.Contains("инженерного лицея"));
		AdministrationByPost(ref workers, e => e is "Главный бухгалтер");
		await ReadPedagogicalWorkersAsync();
		await ManagementReader.Init();
	}

	private static async Task ReadPedagogicalWorkersAsync()
	{
		var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "workers", "pedagogica-workers.xlsx");
		if (!File.Exists(file))
		{
			Logger.Warn("[WorkersReader]: Не удалось найти файл с работниками (pedagogica-workers.xlsx)");
			return;
		}

		await using var stream = File.Open(file, FileMode.Open, FileAccess.Read);

		var excel = new ExcelMapper
		{
			HeaderRow = true,
			CreateMissingHeaders = true
		};

		var reader = await excel.FetchAsync<Worker>(stream);
		var workers = reader.ToList();
		Workers = workers.OrderBy(e => e.FullName).Split(14);
	}

	private static void AdministrationByPost(ref List<Worker> workers, Func<string, bool> predicate)
	{
		Administration.Add(workers.Where(e => predicate.Invoke(e.Post)).ToList());
		workers.RemoveAll(e => predicate.Invoke(e.Post));
	}
}
