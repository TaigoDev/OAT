using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.education.newWorkshops2022
{
    public class MechatronicsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public MechatronicsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}