using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.parents
{
	public class PsychologicalPedagogicalSupportModel : PageModel
	{
		private readonly ILogger<PsychologicalPedagogicalSupportModel> _logger;

		public PsychologicalPedagogicalSupportModel(ILogger<PsychologicalPedagogicalSupportModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}