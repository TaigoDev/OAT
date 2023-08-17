using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ProgramsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ProgramsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}