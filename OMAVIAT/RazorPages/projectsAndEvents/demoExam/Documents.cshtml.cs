using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.demoExam;

public class DocumentsModel : PageModel
{
	private readonly ILogger<DocumentsModel> _logger;

	public DocumentsModel(ILogger<DocumentsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}