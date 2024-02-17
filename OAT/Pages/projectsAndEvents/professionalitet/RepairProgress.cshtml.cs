using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class RepairProgressModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public RepairProgressModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}