using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class AnotherModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public AnotherModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}