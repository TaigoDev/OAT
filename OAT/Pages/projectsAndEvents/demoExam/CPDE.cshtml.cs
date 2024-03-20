using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class CPDEModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public CPDEModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}