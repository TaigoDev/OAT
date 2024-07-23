using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class EducationalStandardsModel : PageModel
	{
		private readonly ILogger<EducationalStandardsModel> _logger;

		public EducationalStandardsModel(ILogger<EducationalStandardsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}