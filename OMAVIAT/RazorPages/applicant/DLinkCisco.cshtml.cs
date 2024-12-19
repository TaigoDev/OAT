using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant {
	public class DLinkCiscoModel : PageModel {
		private readonly ILogger<DLinkCiscoModel> _logger;

		public DLinkCiscoModel(ILogger<DLinkCiscoModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
