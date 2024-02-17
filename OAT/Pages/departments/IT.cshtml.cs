using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.departments
{
	public class ITModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ITModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}