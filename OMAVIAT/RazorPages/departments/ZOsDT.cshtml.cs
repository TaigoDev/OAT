using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.departments {
	public class ZOsDTModel : PageModel {
		private readonly ILogger<ZOsDTModel> _logger;

		public ZOsDTModel(ILogger<ZOsDTModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
