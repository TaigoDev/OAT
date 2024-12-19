using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages {
	public class DemographyModel : PageModel {
		private readonly ILogger<DemographyModel> _logger;

		public DemographyModel(ILogger<DemographyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
