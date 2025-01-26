using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.professionals;

public class LinksModel : PageModel
{
	private readonly ILogger<LinksModel> _logger;

	public LinksModel(ILogger<LinksModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}