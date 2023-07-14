using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
    public class StudentSupportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public StudentSupportModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}