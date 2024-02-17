using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class IndividualTrainingModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public IndividualTrainingModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}