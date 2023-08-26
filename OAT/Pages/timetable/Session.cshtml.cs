using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    [NoCache]
    public class SessionModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public SessionModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}