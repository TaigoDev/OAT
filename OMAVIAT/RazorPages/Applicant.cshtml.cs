using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages {
	public class ApplicantModel : PageModel {
		private readonly ILogger<ApplicantModel> _logger;

		public ApplicantModel(ILogger<ApplicantModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
