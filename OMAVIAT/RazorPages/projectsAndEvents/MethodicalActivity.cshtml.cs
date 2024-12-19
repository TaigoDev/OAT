using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages {
	public class MethodicalActivityModel : PageModel {
		private readonly ILogger<MethodicalActivityModel> _logger;

		public MethodicalActivityModel(ILogger<MethodicalActivityModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
