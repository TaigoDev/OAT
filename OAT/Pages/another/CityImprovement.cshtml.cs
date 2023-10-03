using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.another
{
    public class CityImprovementModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public CityImprovementModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}