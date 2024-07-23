using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.organizationInformation
{
	public class InternationalCooperationModel : PageModel
	{
		private readonly ILogger<InternationalCooperationModel> _logger;

		public InternationalCooperationModel(ILogger<InternationalCooperationModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}