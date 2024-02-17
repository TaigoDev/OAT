using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class CommonIntelligenceModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public CommonIntelligenceModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}