using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
    public class AlumniEmploymentCenterOmskModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public AlumniEmploymentCenterOmskModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}