using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
    public class TrainingCoursesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public TrainingCoursesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}