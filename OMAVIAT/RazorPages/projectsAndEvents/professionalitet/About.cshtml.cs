using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.demoExam
{
	public class AboutModel : PageModel
	{
		private readonly ILogger<AboutModel> _logger;

		public AboutModel(ILogger<AboutModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}