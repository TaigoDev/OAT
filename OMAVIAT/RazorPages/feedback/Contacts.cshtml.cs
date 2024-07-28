using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
	public class ContactsModel : PageModel
	{
		private readonly ILogger<ContactsModel> _logger;

		public ContactsModel(ILogger<ContactsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}