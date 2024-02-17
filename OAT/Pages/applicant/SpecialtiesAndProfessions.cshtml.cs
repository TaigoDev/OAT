using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class SpecialtiesAndProfessionsModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public SpecialtiesAndProfessionsModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}