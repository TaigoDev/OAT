using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class ClassesChangesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ClassesChangesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}