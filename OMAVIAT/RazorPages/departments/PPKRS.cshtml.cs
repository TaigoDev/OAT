using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
{
	public class PPKRSModel : PageModel
	{
		private readonly ILogger<PPKRSModel> _logger;

		public PPKRSModel(ILogger<PPKRSModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}