using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TMModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public TMModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}