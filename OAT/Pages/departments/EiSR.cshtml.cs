using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class EiSRModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public EiSRModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}