using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
	public class ParentMeetingsModel : PageModel
	{
		private readonly ILogger<ParentMeetingsModel> _logger;

		public ParentMeetingsModel(ILogger<ParentMeetingsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}