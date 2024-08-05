using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
{
	public class TicketToFutureModel : PageModel
	{
		private readonly ILogger<TicketToFutureModel> _logger;

		public TicketToFutureModel(ILogger<TicketToFutureModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}