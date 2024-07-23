using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class AnotherModel : PageModel
	{
		private readonly ILogger<AnotherModel> _logger;

		public AnotherModel(ILogger<AnotherModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}