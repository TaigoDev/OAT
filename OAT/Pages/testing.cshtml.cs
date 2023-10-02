using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class testingModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public testingModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}