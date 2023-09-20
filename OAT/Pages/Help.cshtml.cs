using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class HelpModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public HelpModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}