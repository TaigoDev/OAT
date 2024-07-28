using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class SportClubModel : PageModel
	{
		private readonly ILogger<SportClubModel> _logger;

		public SportClubModel(ILogger<SportClubModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}