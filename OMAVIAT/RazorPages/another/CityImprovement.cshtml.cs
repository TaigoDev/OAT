using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.another
{
	public class CityImprovementModel : PageModel
	{
		private readonly ILogger<CityImprovementModel> _logger;

		public CityImprovementModel(ILogger<CityImprovementModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}