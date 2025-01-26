using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class ProjectsAndEventsModel : PageModel
{
	private readonly ILogger<ProjectsAndEventsModel> _logger;

	public ProjectsAndEventsModel(ILogger<ProjectsAndEventsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}