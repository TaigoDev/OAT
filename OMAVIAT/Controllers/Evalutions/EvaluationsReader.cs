using System.Diagnostics;
using System.Globalization;
using System.Text.RegularExpressions;
using Ganss.Excel;
using NLog;
using OMAVIAT.Entities.Journal;

namespace OMAVIAT.Controllers.Evalutions;

public abstract class EvaluationsReader
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task<Student?> Search(string group, string FullName, string month)
	{
		var stopWatch = new Stopwatch();
		stopWatch.Start();

		var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "journal");

		var xlsx = Path.Combine(folder, GetFileName(folder, group));
		if (!File.Exists(xlsx))
			return null;

		var sheets = new ExcelMapper(xlsx).FetchSheetNames().ToList();
		var excel = new ExcelMapper
		{
			HeaderRow = true,
			HeaderRowNumber = 0,
			MinRowNumber = 1,
			CreateMissingHeaders = true,
			SkipBlankCells = false
		};
		var monthName = sheets.FirstOrDefault(e => e.ToLower() == month.ToLower());
		if (!DateTime.TryParseExact(month, "MMMM", CultureInfo.GetCultureInfo("ru-ru"), DateTimeStyles.None,
			    out var dateTime) || monthName is null)
			return null;
		MappingDays(ref excel, DateTime.DaysInMonth(DateTime.Now.Year, dateTime.Month));

		var rawRecords = await excel.FetchAsync<RawRecord>(xlsx, monthName);
		var student = Student.Convert(FullName, rawRecords.ToList());
		stopWatch.Stop();
		Logger.Info($"Оценки пользователя загружены за {stopWatch.ElapsedMilliseconds}ms");
		return student;
	}


	private static string GetFileName(string folder, string group)
	{
		var matches = Regex.Matches(group, @"(\p{L}+)\s*([\d-]+)");
		var letters = matches.First().Groups[1].Value;
		var numbers = matches.First().Groups[2].Value.Remove(0, 1);

		var files = Directory.GetFiles(folder, "*.xlsx").Where(e =>
		{
			var info = new FileInfo(e).Name;
			return info.Contains(letters) && GetGroupNumbers(info).Contains(numbers);
		});

		var enumerable = files.ToList();
		if (enumerable.Count != 0)
			return enumerable.FirstOrDefault(e =>
				new FileInfo(e).Name.Replace(".xlsx", "").Length == group.Length) ?? "";

		return "";
	}

	private static string GetGroupNumbers(string name)
	{
		var matches = Regex.Matches(name, @"(\p{L}+)\s*([\d-]+)");
		return matches.First().Groups[2].Value.Remove(0, 1);
	}

	private static void MappingDays(ref ExcelMapper excel, int days)
	{
		for (var i = 1; i <= days + 1; i++)
			excel.AddMapping<RawRecord>(2 + i, e => e.Cache);
	}


	public abstract class RawRecord
	{
		[Column("Студент")] public string FullName { get; set; } = "ФИО";

		[Column("Дисциплина")] public string Discipline { get; set; } = "Дисциплина";

		public List<string> Marks { get; set; } = [];

		public string? Cache
		{
			get => null;
			set => Marks.Add(value ?? "");
		}
	}
}