using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class DemographyModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DemographyModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}