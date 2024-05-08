using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class YearOfFamilyModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public YearOfFamilyModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}