using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
    public class PayModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PayModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}