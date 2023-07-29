using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ProjectsAndEventsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ProjectsAndEventsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}