using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class NewWorkshops2022Model : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public NewWorkshops2022Model(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}