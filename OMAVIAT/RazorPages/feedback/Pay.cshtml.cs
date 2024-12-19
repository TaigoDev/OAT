using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.feedback {
	public class PayModel : PageModel {
		private readonly ILogger<PayModel> _logger;

		public PayModel(ILogger<PayModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
