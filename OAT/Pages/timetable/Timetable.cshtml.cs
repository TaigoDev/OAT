using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class TimetableModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TimetableModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}