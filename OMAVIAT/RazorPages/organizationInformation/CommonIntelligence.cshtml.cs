using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class CommonIntelligenceModel : PageModel
	{
		private readonly ILogger<CommonIntelligenceModel> _logger;

		public CommonIntelligenceModel(ILogger<CommonIntelligenceModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}