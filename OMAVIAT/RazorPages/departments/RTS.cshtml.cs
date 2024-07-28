using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class RTSModel : PageModel
	{
		private readonly ILogger<RTSModel> _logger;

		public RTSModel(ILogger<RTSModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}