using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class RTSModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public RTSModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}