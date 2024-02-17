using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class FinancialEconomicActivityModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public FinancialEconomicActivityModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}