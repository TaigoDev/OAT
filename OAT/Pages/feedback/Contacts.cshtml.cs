using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
    public class ContactsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public ContactsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}