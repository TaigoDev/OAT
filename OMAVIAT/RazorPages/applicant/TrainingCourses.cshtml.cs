using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant {
	public class TrainingCoursesModel : PageModel {
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
