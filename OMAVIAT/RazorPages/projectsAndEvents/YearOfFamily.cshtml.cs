using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class YearOfFamilyModel : PageModel
	{
		private readonly ILogger<YearOfFamilyModel> _logger;

		public YearOfFamilyModel(ILogger<YearOfFamilyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}