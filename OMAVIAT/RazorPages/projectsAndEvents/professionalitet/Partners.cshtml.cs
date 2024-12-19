using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages {
	public class PartnersModel : PageModel {
		private readonly ILogger<PartnersModel> _logger;
		public PartnersModel(ILogger<PartnersModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
