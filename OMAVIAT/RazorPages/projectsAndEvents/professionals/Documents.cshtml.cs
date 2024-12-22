using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.professionals {
	public class DocumentsModel : PageModel {
		private readonly ILogger<DocumentsModel> _logger;

		public DocumentsModel(ILogger<DocumentsModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
