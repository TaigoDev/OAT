using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using OAT.Readers;

namespace OAT.Pages.timetable
{
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
            groups = ScheduleReader.GetGroupsByBuilding(building);
            max_curse = groups!.Count == 0 ? 0 : groups.Max(e => e.curse);
            this.building = building;
        }
    }
}