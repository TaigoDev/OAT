using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class DuckModel : PageModel
	{
		private readonly ILogger<DuckModel> _logger;

		public DuckModel(ILogger<DuckModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}