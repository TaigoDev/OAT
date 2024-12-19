using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students {
	public class StudentSafetyModel : PageModel {
		private readonly ILogger<StudentSafetyModel> _logger;

		public StudentSafetyModel(ILogger<StudentSafetyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
