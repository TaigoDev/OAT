using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class ResearchWorkModel : PageModel
	{
		private readonly ILogger<ResearchWorkModel> _logger;

		public ResearchWorkModel(ILogger<ResearchWorkModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}