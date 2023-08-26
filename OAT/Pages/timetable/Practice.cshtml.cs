using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    [NoCache]
    public class PracticeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PracticeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}