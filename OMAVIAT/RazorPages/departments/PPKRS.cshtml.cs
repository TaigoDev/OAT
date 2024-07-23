using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class PPKRSModel : PageModel
	{
		private readonly ILogger<PPKRSModel> _logger;

		public PPKRSModel(ILogger<PPKRSModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}