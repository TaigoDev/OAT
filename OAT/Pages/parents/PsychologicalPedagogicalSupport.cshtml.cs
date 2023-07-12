using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
    public class PsychologicalPedagogicalSupportModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public PsychologicalPedagogicalSupportModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}