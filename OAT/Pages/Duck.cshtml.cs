using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class DuckModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public DuckModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}