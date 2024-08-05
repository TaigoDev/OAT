using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.parents
{
	public class EducationalProcessOrganizationModel : PageModel
	{
		private readonly ILogger<EducationalProcessOrganizationModel> _logger;

		public EducationalProcessOrganizationModel(ILogger<EducationalProcessOrganizationModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}