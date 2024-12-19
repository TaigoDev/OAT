using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation {
	public class VacanciesModel : PageModel {
		private readonly ILogger<VacanciesModel> _logger;

		public VacanciesModel(ILogger<VacanciesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
