using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class FinancialEconomicActivityModel : PageModel
	{
		private readonly ILogger<FinancialEconomicActivityModel> _logger;

		public FinancialEconomicActivityModel(ILogger<FinancialEconomicActivityModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}