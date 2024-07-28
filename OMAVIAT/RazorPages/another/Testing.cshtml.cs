using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.another
{
	public class TestingModel : PageModel
	{
		private readonly ILogger<TestingModel> _logger;

		public TestingModel(ILogger<TestingModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}