using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.education.newWorkshops2022
{
	public class ServiceModel : PageModel
	{
		private readonly ILogger<ServiceModel> _logger;

		public ServiceModel(ILogger<ServiceModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}