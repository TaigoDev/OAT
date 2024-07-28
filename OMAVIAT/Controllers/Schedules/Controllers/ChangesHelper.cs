using OAT.Controllers.Schedules.Readers;
using OAT.Entities.Schedule;

namespace OAT.Controllers.Schedules.Controllers
{
	public class ChangesHelper
	{

		public static string GetDate(int corpus, string? sheet)
		{
			var changes = ChangesController.GetListChanges(corpus);
			if (sheet is null)
				return changes.Last().Date;

			var day = changes.FirstOrDefault(e => e.SheetName == sheet);
			if (day is null)
				return "Лист не найден";
			return day.Date;
		}

		public static string GetSchoolWeek(int corpus, string? sheet)
		{
			var changes = ChangesController.GetListChanges(corpus);
			if (sheet is null)
				return changes.Last().SchoolWeek;

			var day = changes.FirstOrDefault(e => e.SheetName == sheet);
			if (day is null)
				return "Лист не найден";
			return day.SchoolWeek;
		}

		public static IEnumerable<Bell> GetBells(int corpus, string? sheet)
		{
			var changes = ChangesController.GetListChanges(corpus);
			if (sheet is null)
				return changes.Last().bells;

			var day = changes.FirstOrDefault(e => e.SheetName == sheet);
			if (day is null)
				return new List<Bell>();
			return day.bells;
		}

		public static List<string> GetDaysList(int corpus)
		{
			var changes = ChangesController.GetListChanges(corpus);
			return changes.ConvertAll(e => e.SheetName);
		}

		public static string GetSheetName(int corpus)
		{
			var changes = ChangesController.GetListChanges(corpus);
			return changes.Last().SheetName;
		}

		//public static List<ChangeRow> GetChangeRows(int corpus, IQueryCollection query)
		//{
		//	var search = query.FirstOrDefault(e => e.Key == "search").Value.ToString().ToLower();
		//	ChangesController.GetListChanges(corpus).Where(e => )
		//}
	}
}
