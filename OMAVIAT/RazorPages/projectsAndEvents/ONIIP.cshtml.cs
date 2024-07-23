using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class ONIIPModel : PageModel
	{
		private readonly ILogger<ONIIPModel> _logger;

		public ONIIPModel(ILogger<ONIIPModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}