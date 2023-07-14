using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
    public class EducationalStandardsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public EducationalStandardsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}