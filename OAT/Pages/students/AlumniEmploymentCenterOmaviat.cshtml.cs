using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
    public class AlumniEmploymentCenterOmaviatModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public AlumniEmploymentCenterOmaviatModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}