using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.education.newWorkshops2022 {
	public class ElectronicsModel : PageModel {
		private readonly ILogger<ElectronicsModel> _logger;

		public ElectronicsModel(ILogger<ElectronicsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
