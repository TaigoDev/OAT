using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.timetable
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