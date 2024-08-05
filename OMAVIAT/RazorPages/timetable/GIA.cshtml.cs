using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.timetable
{
	public class GIAModel : PageModel
	{
		private readonly ILogger<GIAModel> _logger;

		public GIAModel(ILogger<GIAModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}