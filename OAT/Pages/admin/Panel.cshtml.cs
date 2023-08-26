using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    [Authorize, NoCache]
    public class PanelModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PanelModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            if (User == null || !User.Identity.IsAuthenticated)
                Redirect("admin/authorization");
        }
    }
}