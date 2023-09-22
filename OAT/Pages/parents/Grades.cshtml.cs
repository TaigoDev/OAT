using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
    public class GradesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public GradesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}