using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
	public class MapModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public MapModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}