using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Schedule.Services.MainSchedule;

namespace OMAVIAT.Pages.timetable {
	[NoCache]
	public class TimetableModel : PageModel {
		private readonly ILogger<TimetableModel> _logger;

		public TimetableModel(ILogger<TimetableModel> logger)
		{
			_logger = logger;
		}
		public string? building { get; set; }
		public string? group_name { get; set; }
		public Entities.Schedule.Schedule? group { get; set; }
		public CorpusSchedule? corpus { get; set; }

		public void OnGet(string? building, string? group_name)
		{
			this.building = building;
			this.group_name = group_name;
			if (!Enum.TryParse<Building>(building, true, out var result))
				return;

			corpus = ScheduleReader.schedules.FirstOrDefault(e => e.building == (int)result);
			if (corpus is null)
				return;
			group = corpus.groups.FirstOrDefault(e => e.name == group_name);
		}

		public List<List<ScheduleLesson>> GetAllLessonsByNumber(int id, ScheduleWeek week)
		{
			var lessons = new List<List<ScheduleLesson>>();
			foreach (var day in week.Days)
				lessons.Add(day.lessons.Where(e => e.Id == id).ToList());

			return lessons;
		}

		public int GetMaxLessonsInDay(ScheduleWeek week) =>
			week.Days.Max(e => e.lessons.Count == 0 ? 0 : e.lessons.Max(l => l.Id));
		public int GetMinLessonsInDay(ScheduleWeek week) =>
			week.Days.Min(e => e.lessons.Count == 0 ? 100 : e.lessons.Min(l => l.Id));
	}
}
