using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class AccessibleEnvironmentModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public AccessibleEnvironmentModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}