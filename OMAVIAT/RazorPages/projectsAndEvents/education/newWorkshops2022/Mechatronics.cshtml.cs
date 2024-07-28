using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.education.newWorkshops2022
{
	public class MechatronicsModel : PageModel
	{
		private readonly ILogger<MechatronicsModel> _logger;

		public MechatronicsModel(ILogger<MechatronicsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}