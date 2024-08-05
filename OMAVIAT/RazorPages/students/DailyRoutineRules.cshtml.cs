using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students
{
	public class DailyRoutineRulesModel : PageModel
	{
		private readonly ILogger<DailyRoutineRulesModel> _logger;

		public DailyRoutineRulesModel(ILogger<DailyRoutineRulesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}