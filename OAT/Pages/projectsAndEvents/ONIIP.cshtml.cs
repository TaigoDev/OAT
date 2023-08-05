using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ONIIPModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ONIIPModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}