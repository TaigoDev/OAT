using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.departments
{
	public class ITModel : PageModel
	{
		private readonly ILogger<ITModel> _logger;

		public ITModel(ILogger<ITModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}