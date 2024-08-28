using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Schedule.Schedule.MainSchedule;

namespace OMAVIAT.Pages.timetable
{
	[NoCache]
	public class TeacherScheduleModel : PageModel
	{

		public Entities.Schedule.Schedule? teacher;
		public CorpusSchedule? corpus;
		public string? fullname;
		public void OnGet(string building, string fullname)
		{
			this.fullname = fullname;
			if (fullname is null)
				return;
			if (!Enum.TryParse<Building>(building, true, out var result))
				return;

			corpus = ScheduleReader.schedules.FirstOrDefault(e => e.building == (int)result);
			if (corpus == null)
				return;
			teacher = corpus.teachers.FirstOrDefault(e => e.name.ToSearchView() == fullname.ToSearchView());
		}

		public List<ScheduleLesson?> GetAllLessonsByNumber(int id, ScheduleWeek week)
		{
			var lessons = new List<ScheduleLesson?>();
			foreach (var day in week.Days.OrderBy(e => e.Type))
			{
				var lesson = day.lessons.FirstOrDefault(e => e.Id == id);
				lessons.Add(lesson);
			}
			return lessons;
		}

		public int GetMaxLessonsInDay(ScheduleWeek week) =>
			week.Days.Max(e => e.lessons.Count == 0 ? 0 : e.lessons.Max(l => l.Id));

		public int GetMinLessonsInDay(ScheduleWeek week) =>
			week.Days.Min(e => e.lessons.Count == 0 ? 100 : e.lessons.Min(l => l.Id));
	}
}
