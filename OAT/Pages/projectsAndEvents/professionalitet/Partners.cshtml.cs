using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class PartnersModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PartnersModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}