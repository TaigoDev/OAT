using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class LibraryModel : PageModel
	{
		private readonly ILogger<LibraryModel> _logger;

		public LibraryModel(ILogger<LibraryModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}