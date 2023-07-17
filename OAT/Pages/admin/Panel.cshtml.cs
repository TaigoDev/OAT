using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    [Authorize]
    public class PanelModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PanelModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}