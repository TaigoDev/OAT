using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.feedback
{
	public class LocationModel : PageModel
	{
		private readonly ILogger<LocationModel> _logger;

		public LocationModel(ILogger<LocationModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}