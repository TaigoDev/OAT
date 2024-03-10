using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Entities.Schedule;
using OAT.Modules.Schedules;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class TeacherScheduleModel : PageModel
	{

		public TeacherSchedule? teacher;
		public string fullname;
		public void OnGet(string building, string fullname)
		{
			this.fullname = fullname;
			if (fullname is null)
				return;
			teacher = ScheduleUtils.GetTeacherScheduleByBuilding(building).FirstOrDefault(e => e.FullName.ToLower() == fullname.ToLower());
		}

		public List<TeacherLesson> GetAllLessonsByNumber(int id, TeacherWeek week)
		{
			var lessons = new List<TeacherLesson>();
			foreach (var day in week.days)
				lessons.Add(day.lessons.FirstOrDefault(e => e.id == id)!);
			return lessons;
		}

		public int GetMaxLessonsInDay(TeacherWeek week) =>
			week.days.Max(e => e.lessons.Count == 0 ? 0 : e.lessons.Max(l => l.id));

		public int GetMinLessonsInDay(TeacherWeek week) =>
			week.days.Min(e => e.lessons.Count == 0 ? 100 : e.lessons.Min(l => l.id));
	}
}
