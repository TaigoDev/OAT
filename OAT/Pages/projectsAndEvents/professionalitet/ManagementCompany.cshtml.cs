using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ManagementCompanyModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ManagementCompanyModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}