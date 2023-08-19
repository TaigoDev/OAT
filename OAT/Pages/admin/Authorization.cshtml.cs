using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class AuthorizationModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public AuthorizationModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}