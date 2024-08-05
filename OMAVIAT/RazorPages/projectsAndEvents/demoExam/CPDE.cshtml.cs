using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages
{
	public class CPDEModel : PageModel
	{
		private readonly ILogger<CPDEModel> _logger;

		public CPDEModel(ILogger<CPDEModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}