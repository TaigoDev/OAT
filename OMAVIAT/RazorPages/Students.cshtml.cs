using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class StudentsModel : PageModel
	{
		private readonly ILogger<StudentsModel> _logger;

		public StudentsModel(ILogger<StudentsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}