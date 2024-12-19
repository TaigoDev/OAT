using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation {
	public class EducationModel : PageModel {
		private readonly ILogger<EducationModel> _logger;

		public EducationModel(ILogger<EducationModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
