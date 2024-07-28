using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class SelectionCommitteeModel : PageModel
	{
		private readonly ILogger<SelectionCommitteeModel> _logger;

		public SelectionCommitteeModel(ILogger<SelectionCommitteeModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}