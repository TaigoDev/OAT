using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.education.newWorkshops2022 {
	public class AssemblyModel : PageModel {
		private readonly ILogger<AssemblyModel> _logger;

		public AssemblyModel(ILogger<AssemblyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
