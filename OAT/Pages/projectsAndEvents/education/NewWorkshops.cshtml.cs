using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class NewWorkshopsModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public NewWorkshopsModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}