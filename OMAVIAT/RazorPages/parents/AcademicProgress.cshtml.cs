using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
	public class AcademicProgressModel : PageModel
	{
		private readonly ILogger<AcademicProgressModel> _logger;

		public AcademicProgressModel(ILogger<AcademicProgressModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}