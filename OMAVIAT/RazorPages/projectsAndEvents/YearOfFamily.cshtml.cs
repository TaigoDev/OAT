using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
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