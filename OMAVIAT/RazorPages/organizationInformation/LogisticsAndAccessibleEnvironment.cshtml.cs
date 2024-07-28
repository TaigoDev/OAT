using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class LogisticsAndAccessibleEnvironmentModel : PageModel
	{
		private readonly ILogger<LogisticsAndAccessibleEnvironmentModel> _logger;

		public LogisticsAndAccessibleEnvironmentModel(ILogger<LogisticsAndAccessibleEnvironmentModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}