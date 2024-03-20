using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class GalleryModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public GalleryModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}