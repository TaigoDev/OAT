using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
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