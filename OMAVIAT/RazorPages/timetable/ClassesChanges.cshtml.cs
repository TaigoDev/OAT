using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class ClassesChangesModel : PageModel
	{
		private readonly ILogger<ClassesChangesModel> _logger;

		public ClassesChangesModel(ILogger<ClassesChangesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}