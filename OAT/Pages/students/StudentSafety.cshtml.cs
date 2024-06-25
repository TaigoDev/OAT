using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class StudentSafetyModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public StudentSafetyModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}