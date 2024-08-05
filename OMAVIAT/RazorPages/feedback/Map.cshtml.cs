using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.feedback
{
	public class MapModel : PageModel
	{
		private readonly ILogger<MapModel> _logger;

		public MapModel(ILogger<MapModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}