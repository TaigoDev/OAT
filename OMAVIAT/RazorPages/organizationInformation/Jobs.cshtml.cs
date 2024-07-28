using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class JobsModel : PageModel
	{
		private readonly ILogger<JobsModel> _logger;

		public JobsModel(ILogger<JobsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}