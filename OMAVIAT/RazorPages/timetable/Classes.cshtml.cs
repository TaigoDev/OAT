using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
	public class ClassesModel : PageModel
	{
		private readonly ILogger<ClassesModel> _logger;

		public ClassesModel(ILogger<ClassesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}