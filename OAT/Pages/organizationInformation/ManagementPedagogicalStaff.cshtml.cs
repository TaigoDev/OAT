using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
    public class ManagementPedagogicalStaffModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ManagementPedagogicalStaffModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}