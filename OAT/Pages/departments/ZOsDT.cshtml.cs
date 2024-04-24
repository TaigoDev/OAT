using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.departments
{
	public class ZOsDTModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ZOsDTModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}