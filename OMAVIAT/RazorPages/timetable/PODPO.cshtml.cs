using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.timetable
{
	[NoCache]
	public class PODPOModel : PageModel
	{
		private readonly ILogger<PODPOModel> _logger;

		public PODPOModel(ILogger<PODPOModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}