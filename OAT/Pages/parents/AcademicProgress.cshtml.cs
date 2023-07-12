using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
    public class AcademicProgressModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public AcademicProgressModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}