using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class LogisticsAndAccessibleEnvironmentModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public LogisticsAndAccessibleEnvironmentModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}