using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
    public class LocationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public LocationModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}