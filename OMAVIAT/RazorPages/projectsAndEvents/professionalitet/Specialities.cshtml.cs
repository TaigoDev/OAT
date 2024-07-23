using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class SpecialitiesModel : PageModel
	{
		private readonly ILogger<SpecialitiesModel> _logger;

		public SpecialitiesModel(ILogger<SpecialitiesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}