using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class TicketToFutureModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public TicketToFutureModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}