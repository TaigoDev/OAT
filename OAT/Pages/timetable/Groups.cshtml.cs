using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.UtilsHelper;

namespace OAT.Pages.timetable
{
    [NoCache]
    public class GroupsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GroupsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public string? building { get; set; }
        public List<Group>? groups { get; set; }
        public int max_curse { get; set; }

        public void OnGet(string? building)
        {
            groups = ScheduleUtils.GetGroupsByBuilding(building);
            if (groups is not null)
                max_curse = groups!.Count == 0 ? 0 : groups.Max(e => e.curse);
            this.building = building;
        }
    }
}