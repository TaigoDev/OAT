using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class TimetableClassesChangesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TimetableClassesChangesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}