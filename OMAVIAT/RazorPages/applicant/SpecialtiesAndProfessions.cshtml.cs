using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class SpecialtiesAndProfessionsModel : PageModel
	{
		private readonly ILogger<SpecialtiesAndProfessionsModel> _logger;

		public SpecialtiesAndProfessionsModel(ILogger<SpecialtiesAndProfessionsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}