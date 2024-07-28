using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TAKPiEModel : PageModel
	{
		private readonly ILogger<TAKPiEModel> _logger;

		public TAKPiEModel(ILogger<TAKPiEModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}