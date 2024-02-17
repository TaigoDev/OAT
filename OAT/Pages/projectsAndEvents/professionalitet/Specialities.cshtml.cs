using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class SpecialitiesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public SpecialitiesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}