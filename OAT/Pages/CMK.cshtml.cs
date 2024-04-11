using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class CMKModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public CMKModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}