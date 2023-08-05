using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
    public class PrintModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PrintModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}