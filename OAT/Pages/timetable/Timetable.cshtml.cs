using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;

namespace OAT.Pages.timetable
{
    public class TimetableModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TimetableModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public string? building { get; set; }
        public string? group_name { get; set; }
        public Group? group { get; set; }

        public void OnGet(string? building, string? group_name)
        {
            this.building = building;
            this.group_name = group_name;
            var groupsInBuilding = ScheduleReader.GetGroupsByBuilding(building);
            group = groupsInBuilding is null || groupsInBuilding.Count == 0 ? null : groupsInBuilding.FirstOrDefault(e => e.name == group_name);
        }

        public List<Lesson> GetAllLessonsByNumber(int id, Week week)
        {
            var lessons = new List<Lesson>();
            foreach (var day in week.days)
                lessons.Add(day.lessons.FirstOrDefault(e => e.id == id)!);
            return lessons;
        }

        public int GetMaxLessonsInDay(Week week) =>
            week.days.Max(e =>
            e.lessons.Count == 0 ? 0 : e.lessons.Max(l => l.id));
        public int GetMinLessonsInDay(Week week) =>
    week.days.Min(e => e.lessons.Count == 0 ? 100 : e.lessons.Min(l => l.id));
    }
}