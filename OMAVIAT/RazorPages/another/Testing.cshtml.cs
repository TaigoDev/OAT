using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another
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