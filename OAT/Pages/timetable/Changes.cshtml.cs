using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ChangesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ChangesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}