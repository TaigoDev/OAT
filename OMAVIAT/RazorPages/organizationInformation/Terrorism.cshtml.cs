using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class TerrorismModel : PageModel
	{
		private readonly ILogger<TerrorismModel> _logger;

		public TerrorismModel(ILogger<TerrorismModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}