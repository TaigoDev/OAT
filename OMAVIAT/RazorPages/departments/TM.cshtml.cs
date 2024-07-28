using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TMModel : PageModel
	{
		private readonly ILogger<TMModel> _logger;

		public TMModel(ILogger<TMModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}