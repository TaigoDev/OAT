using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
    public class TerrorismModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TerrorismModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}