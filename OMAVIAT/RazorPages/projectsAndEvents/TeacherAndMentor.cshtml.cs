using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TeacherAndMentorModel : PageModel
	{
		private readonly ILogger<TeacherAndMentorModel> _logger;

		public TeacherAndMentorModel(ILogger<TeacherAndMentorModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}