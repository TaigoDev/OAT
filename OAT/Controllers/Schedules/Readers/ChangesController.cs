using OAT.Entities.Schedule;
using OAT.Utilities;
using OfficeOpenXml;
using System.Diagnostics;


namespace OAT.Controllers.Schedules.Readers
{
	public class ChangesController
	{
		public static List<Changes> changes_b1 = [];
		public static List<Changes> changes_b2 = [];
		public static List<Changes> changes_b3 = [];
		public static List<Changes> changes_b4 = [];

		public static async Task init()
		{
			var stopWatch = new Stopwatch();
			stopWatch.Start();

			var ids = new[] { 1, 2, 3, 4 };
			await Runs<int>.InTasks(UpdateCorpusChanges, [.. ids], false);
			stopWatch.Stop();
			Logger.Info($"Изменения загружены за {stopWatch.ElapsedMilliseconds} ms");
		}

		public static async Task UpdateCorpusChanges(int index, bool IsRepeat = false)
		{
			var xlsx = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{index}-changes.xlsx");
			if (!File.Exists(xlsx))
				return;

			if (FileUtils.IsFileLocked(xlsx) && !IsRepeat)
			{
				RepeaterUtils.Try(() => UpdateCorpusChanges(index, true), TimeSpan.FromMinutes(10), 3);
				return;
			}

			await using var fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
			var excel = new ExcelPackage(fileStream);
			var corpus = GetListChanges(index);
			corpus.Clear();
			var sheets = excel.Workbook.Worksheets.ToList();
			foreach (var sheet in sheets.Count >= 3 ? sheets.GetRange(sheets.Count - 3, 3) : sheets.GetRange(0, sheets.Count))
			{
				try
				{
					var changes = GetChangesAsync(excel, sheet.Name);
					var bells = GetBellsAsync(excel, sheet.Name);
					if (changes != null)
						corpus.Add(new(sheet.Name, GetSchoolWeek(excel, sheet.Name), GetDateText(excel, sheet.Name), bells, changes.Where(e => e.group is not null)));
				}
				catch (Exception ex)
				{
					Logger.Error($"Произошла ошибка при загрузке листа {sheet} корпус {index}.\n {ex}");
				}
			}
		}

		public static IEnumerable<ChangeRow> GetChangesAsync(ExcelPackage excel, string? sheet = null)
		{
			sheet ??= excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
			var workSheet = excel.Workbook.Worksheets[sheet];
			var newcollection = workSheet.Fetch<ChangeRow>(11, 300);
			return newcollection;
		}

		public static IEnumerable<Bell> GetBellsAsync(ExcelPackage excel, string? sheet = null)
		{
			sheet ??= excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
			var workSheet = excel.Workbook.Worksheets[sheet];
			var newcollection = workSheet.Fetch<Bell>(11, 16);
			return newcollection;
		}

		public static string GetSchoolWeek(ExcelPackage excel, string? sheet = null)
		{
			sheet ??= excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
			var workSheet = excel.Workbook.Worksheets[sheet];
			return workSheet.GetValue<string>(7, 10);
		}

		public static string GetDateText(ExcelPackage excel, string? sheet = null)
		{
			sheet ??= excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
			var workSheet = excel.Workbook.Worksheets[sheet];
			return workSheet.GetValue<string>(8, 1);
		}


		public static List<Changes> GetListChanges(int id) =>
			id switch
			{
				1 => changes_b1,
				2 => changes_b2,
				3 => changes_b3,
				4 => changes_b4,
				_ => []
			};





	}




}
