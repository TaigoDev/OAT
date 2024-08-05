using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
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