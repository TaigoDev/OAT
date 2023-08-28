using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class PPKRSModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PPKRSModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}