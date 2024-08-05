using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant
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