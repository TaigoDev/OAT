using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class AdmissionRulesModel : PageModel
	{
		private readonly ILogger<AdmissionRulesModel> _logger;

		public AdmissionRulesModel(ILogger<AdmissionRulesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}