using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
{
	public class ManagementCompanyModel : PageModel
	{
		private readonly ILogger<ManagementCompanyModel> _logger;

		public ManagementCompanyModel(ILogger<ManagementCompanyModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}