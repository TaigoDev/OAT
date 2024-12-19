using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation {
	public class PaidEducationalServicesModel : PageModel {
		private readonly ILogger<PaidEducationalServicesModel> _logger;

		public PaidEducationalServicesModel(ILogger<PaidEducationalServicesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
