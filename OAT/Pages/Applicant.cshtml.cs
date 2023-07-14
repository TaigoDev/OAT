using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ApplicantModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ApplicantModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}