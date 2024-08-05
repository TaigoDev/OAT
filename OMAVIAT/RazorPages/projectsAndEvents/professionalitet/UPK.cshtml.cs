using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.RazorPages.projectsAndEvents.professionalitet
{
	public class UPKModel : PageModel
	{
		private readonly ILogger<UPKModel> _logger;

		public UPKModel(ILogger<UPKModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}