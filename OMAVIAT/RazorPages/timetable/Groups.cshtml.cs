using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Controllers.Schedules;
using OAT.Entities.Enums;
using OAT.Entities.Schedule;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Services.Schedule.MainSchedule;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class GroupsModel : PageModel
	{
		private readonly ILogger<GroupsModel> _logger;

		public GroupsModel(ILogger<GroupsModel> logger)
		{
			_logger = logger;
		}

		public string? building { get; set; }
		public List<Schedule>? groups { get; set; }
		public int max_curse { get; set; }

		public void OnGet(string? building)
		{
			if (!Enum.TryParse<Building>(building, true, out var result))
				return;
			var corpus = ScheduleReader.schedules.FirstOrDefault(e => e.building == (int)result);
			if (corpus is null)
				return;

			groups = corpus.groups.OrderBy(e => e.Curs).ToList();
			if (groups is not null)
				max_curse = (groups.Count == 0 ? 0 : groups.Max(e => e.Curs)) ?? 4;
			this.building = building;
		}
	}
}