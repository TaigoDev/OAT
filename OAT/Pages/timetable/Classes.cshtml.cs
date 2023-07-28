using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
    public class ClassesModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ClassesModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}