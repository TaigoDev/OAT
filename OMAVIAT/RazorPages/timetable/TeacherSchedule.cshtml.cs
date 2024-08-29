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
			for(var i = 1; i <= 6; i++)
				lessons.Add(null);
			foreach (var day in week.Days.OrderBy(e => e.Type))
			{
				var lesson = day.lessons.Where(e => e.Id == id).ToList();
				if(lesson.Count == 1)
					lessons[day.Type] = lesson.First();
				else if (lesson.Any(e => e.Name.ToSearchView() == "Разгово".ToSearchView()))
				{
					var less = lesson.FirstOrDefault(e => e.Name.ToSearchView() != "Разгово".ToSearchView());
					var raz = lesson.FirstOrDefault(e => e.Name.ToSearchView() == "Разгово".ToSearchView());
					if(less is null || raz is null) continue;
					less.Name = $"{less.Name}/Разговор";
					less.Cabinet = $"{less.Cabinet}/{raz.Cabinet}";
					less.Group = $"{less.Group}/{raz.Group}";
					lessons[day.Type] = less;
				}

			}
			return lessons;
		}

		public int GetMaxLessonsInDay(ScheduleWeek week) =>
			week.Days.Max(e => e.lessons.Count == 0 ? 0 : e.lessons.Max(l => l.Id));

		public int GetMinLessonsInDay(ScheduleWeek week) =>
			week.Days.Min(e => e.lessons.Count == 0 ? 100 : e.lessons.Min(l => l.Id));
	}
}
