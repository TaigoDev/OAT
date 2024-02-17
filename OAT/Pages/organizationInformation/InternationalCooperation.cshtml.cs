using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class InternationalCooperationModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public InternationalCooperationModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}