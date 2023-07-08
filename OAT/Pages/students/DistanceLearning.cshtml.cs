using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
    public class DistanceLearningModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public DistanceLearningModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}