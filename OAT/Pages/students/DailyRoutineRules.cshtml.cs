using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.students
{
	public class DailyRoutineRulesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public DailyRoutineRulesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}