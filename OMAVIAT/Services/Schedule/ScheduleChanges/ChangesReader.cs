using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Utilities;

namespace OMAVIAT.Services.Schedule.ScheduleChanges
{
	public class ChangesReader
	{
		public static DateTime GetMaxDate(List<string> sheets)
		{
			if (!sheets.Any())
				return new(1990, 1, 1);
			return sheets.Max(e => DateTime.ParseExact(ToCorrectSheetName(e), "dd.MM", null));
		}

		public static async Task<IEnumerable<ChangeRow>> GetChangesAsync(string xlsx, string? sheet = null)
		{
			await using FileStream fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
			var excel = new ExcelPackage(fileStream);
			return GetChangesAsync(excel, sheet);
		}

		public static IEnumerable<ChangeRow> GetChangesAsync(ExcelPackage excel, string? sheet = null)
		{
			if (sheet is null)
				sheet = GetSheetName(excel.Workbook.Worksheets);
			var workSheet = excel.Workbook.Worksheets[sheet];
			var newcollection = workSheet.Fetch<ChangeRow>(11, 300);
			return newcollection;
		}
		public static async Task<IEnumerable<Bell>> GetBellsAsync(string xlsx, string? sheet = null)
		{
			await using FileStream fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
			var excel = new ExcelPackage(fileStream);
			return GetBellsAsync(excel, sheet);
		}
		public static IEnumerable<Bell> GetBellsAsync(ExcelPackage excel, string? sheet = null)
		{
			if (sheet is null)
				sheet = GetSheetName(excel.Workbook.Worksheets);
			var workSheet = excel.Workbook.Worksheets[sheet];
			var newcollection = workSheet.Fetch<Bell>(11, 16);
			return newcollection;
		}


		public static string GetDayType(ExcelPackage excel, string? sheet = null)
		{
			if (sheet is null)
				sheet = GetSheetName(excel.Workbook.Worksheets);
			var workSheet = excel.Workbook.Worksheets[sheet];
			var collection = workSheet.Fetch<DayType>(17, 17);
			return string.IsNullOrWhiteSpace(collection.First().type) ? "" : collection.First().type;
		}
		public static List<string> GetSheetsName(string xlsx)
		{
			if (!File.Exists(xlsx))
				return new();
			using FileStream fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
			var excel = new ExcelPackage(fileStream);
			return excel.Workbook.Worksheets.ToList().ConvertAll(e => e.Name);
		}

		public static string GetSheetName(ExcelWorksheets sheets)
		{
			var max_sheet = sheets!.Max(e => DateTime.ParseExact(ToCorrectSheetName(e.Name), "d.MM", null));
			return sheets.First(e => DateTime.ParseExact(ToCorrectSheetName(e.Name), "d.MM", null) == max_sheet).Name;
		}
		public static string GetSheetName(List<string> sheets)
		{
			var max_sheet = sheets!.Max(e => DateTime.ParseExact(ToCorrectSheetName(e), "d.MM", null));
			return ToCorrectSheetName(sheets.First(e => DateTime.ParseExact(ToCorrectSheetName(e), "d.MM", null) == max_sheet));
		}
		public static string ToCorrectSheetName(string sheet)
		{
			var letters = "1234567890.";
			var correct = sheet.ToList();
			correct.RemoveAll(e => !letters.Contains(e));
			correct[correct.FindIndex(e => e is '.')] = ':';
			sheet = new string(correct.ToArray());
			return sheet.Replace(".", "").Replace(":", ".");
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

		public static async Task<DateOnly> GetMaxDate(Corpus corpus)
		{
			using var db = new DatabaseContext();
			var maxdate = await db.daysChanges.Where(e => e.corpus == corpus).MaxAsync(e => e.date);
			return maxdate;
		}
	}
}
