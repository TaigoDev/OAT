using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.another
{
    public class ImportantForStudentsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ImportantForStudentsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}