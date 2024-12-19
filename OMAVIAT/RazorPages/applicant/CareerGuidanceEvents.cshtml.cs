using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant {
	public class CareerGuidanceEventsModel : PageModel {
		private readonly ILogger<CareerGuidanceEventsModel> _logger;

		public CareerGuidanceEventsModel(ILogger<CareerGuidanceEventsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
