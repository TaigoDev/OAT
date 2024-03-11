using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class ResearchWorkModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ResearchWorkModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}