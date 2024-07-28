using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class TrainingCoursesModel : PageModel
	{
		private readonly ILogger<TrainingCoursesModel> _logger;

		public TrainingCoursesModel(ILogger<TrainingCoursesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}