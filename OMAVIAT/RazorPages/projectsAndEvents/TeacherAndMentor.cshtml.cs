using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
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