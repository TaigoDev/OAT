using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
    public class DLinkCiscoModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DLinkCiscoModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}