using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class PODPOModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public PODPOModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}