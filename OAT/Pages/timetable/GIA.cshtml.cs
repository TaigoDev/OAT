using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class GIAModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GIAModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}