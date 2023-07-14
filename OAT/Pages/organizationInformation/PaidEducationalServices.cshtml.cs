using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
    public class PaidEducationalServicesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PaidEducationalServicesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}