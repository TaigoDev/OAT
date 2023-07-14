using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class TimetablePracticeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TimetablePracticeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}