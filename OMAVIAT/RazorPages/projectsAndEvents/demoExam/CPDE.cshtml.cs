using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class CPDEModel : PageModel
	{
		private readonly ILogger<CPDEModel> _logger;

		public CPDEModel(ILogger<CPDEModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}