using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
    public class EducationalProcessOrganizationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public EducationalProcessOrganizationModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}