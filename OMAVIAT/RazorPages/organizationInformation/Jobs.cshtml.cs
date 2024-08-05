using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation
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