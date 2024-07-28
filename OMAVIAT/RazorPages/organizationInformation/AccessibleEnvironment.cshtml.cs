using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class AccessibleEnvironmentModel : PageModel
	{
		private readonly ILogger<AccessibleEnvironmentModel> _logger;

		public AccessibleEnvironmentModel(ILogger<AccessibleEnvironmentModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}