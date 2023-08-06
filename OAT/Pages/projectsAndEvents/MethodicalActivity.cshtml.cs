using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class MethodicalActivityModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public MethodicalActivityModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}