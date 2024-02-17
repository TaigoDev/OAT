using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
	public class ParentMeetingsModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ParentMeetingsModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}