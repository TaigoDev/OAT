using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.demoExam
{
	public class DocumentsModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public DocumentsModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}