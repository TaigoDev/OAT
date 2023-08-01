using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ProjectNewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public ProjectNewsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}