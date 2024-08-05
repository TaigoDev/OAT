using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation
{
	public class FoodModel : PageModel
	{
		private readonly ILogger<FoodModel> _logger;

		public FoodModel(ILogger<FoodModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}