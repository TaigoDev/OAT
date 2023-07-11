using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
    public class CareerGuidanceEventsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public CareerGuidanceEventsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}