using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class EducationalProcessModel : PageModel
	{
		private readonly ILogger<EducationalProcessModel> _logger;

		public EducationalProcessModel(ILogger<EducationalProcessModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}