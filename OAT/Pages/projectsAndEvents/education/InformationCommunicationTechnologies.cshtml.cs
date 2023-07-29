using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.education
{
    public class InformationCommunicationTechnologiesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public InformationCommunicationTechnologiesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}