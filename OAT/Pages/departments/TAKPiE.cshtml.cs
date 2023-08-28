using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class TAKPiEModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TAKPiEModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}