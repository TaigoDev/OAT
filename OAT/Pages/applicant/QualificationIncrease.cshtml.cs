using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
    public class QualificationIncreaseModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public QualificationIncreaseModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}