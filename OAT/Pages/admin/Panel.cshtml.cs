using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    [AuthorizeRoles(Enums.Role.admin, Enums.Role.reporter, Enums.Role.manager)]
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