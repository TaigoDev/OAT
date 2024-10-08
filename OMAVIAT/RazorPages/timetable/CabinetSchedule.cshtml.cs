using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Schedule.Services.MainSchedule;
using OMAVIAT.Services.Schedule.MainSchedule;

namespace OMAVIAT.RazorPages.timetable
{
	[NoCache]
	public class CabinetScheduleModel : PageModel
	{
		public Entities.Schedule.Schedule? cabinet;
		public CorpusSchedule? corpus;
		public string? cabinet_text;
		public void OnGet(string building, string cabinet_text)
		{
			this.cabinet_text = cabinet_text;
			if (cabinet_text is null)
				return;
			if (!Enum.TryParse<Building>(building, true, out var result))
				return;

			corpus = ScheduleReader.schedules.FirstOrDefault(e => e.building == (int)result);
			if (corpus == null)
				return;
			cabinet = corpus.cabinets.FirstOrDefault(e => e.name.ToSearchView() == cabinet_text.ToSearchView());
		}

		public List<ScheduleLesson?> GetAllLessonsByNumber(int id, ScheduleWeek week)
		{
			var lessons = new List<ScheduleLesson?>();
			for(var i = 1; i <= 6; i++)
				lessons.Add(null);
			foreach (var day in week.Days.OrderBy(e => e.Type))
			{
				var lesson = day.lessons.Where(e => e.Id == id).ToList();
				switch (lesson.Count)
				{
					case 1:
						lessons[day.Type - 1] = lesson.First();
						break;
					case 2:
					{
						var less = lesson.FirstOrDefault();
						var raz = lesson.LastOrDefault();
						if(less is null || raz is null) continue;
						var combine = new ScheduleLesson()
						{
							Id = less.Id,
							Cabinet =  $"{less.Cabinet}/{raz.Cabinet}",
							subGroupId = less.subGroupId,
							FullName = less.FullName,
							Teacher = less.Teacher,
							Corpus = less.Corpus,
							Group = $"{less.Group}/{raz.Group}",
							Name = $"{less.Name}/{raz.Name}",
						};
						lessons[day.Type] = combine;
						break;
					}
					default:
					{
						if(lesson.Count != 0)
						{
							lessons[day.Type] = lesson.First();
						}

						break;
					}
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
