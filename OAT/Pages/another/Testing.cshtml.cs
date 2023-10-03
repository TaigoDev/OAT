using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.another
{
    public class TestingModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TestingModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}