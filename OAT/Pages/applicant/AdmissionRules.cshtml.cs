using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class AdmissionRulesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public AdmissionRulesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}