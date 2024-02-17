using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class SportClubModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public SportClubModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}