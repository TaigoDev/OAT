using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TeacherAndMentorModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public TeacherAndMentorModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}