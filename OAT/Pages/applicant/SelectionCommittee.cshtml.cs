using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
    public class SelectionCommitteeModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public SelectionCommitteeModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}