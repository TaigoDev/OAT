using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class GroupsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GroupsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}