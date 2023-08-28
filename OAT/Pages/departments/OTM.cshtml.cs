using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class OTMModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public OTMModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}