using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.education
{
	public class InformationCommunicationTechnologiesModel : PageModel
	{
		private readonly ILogger<InformationCommunicationTechnologiesModel> _logger;

		public InformationCommunicationTechnologiesModel(ILogger<InformationCommunicationTechnologiesModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}