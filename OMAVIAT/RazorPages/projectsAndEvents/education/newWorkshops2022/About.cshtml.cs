using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.education.newWorkshops2022
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