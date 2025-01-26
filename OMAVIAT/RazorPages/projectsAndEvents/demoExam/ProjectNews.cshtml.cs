using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.demoExam;

public class ProjectNewsModel(ILogger<ProjectNewsModel> logger) : PageModel
{
	private readonly ILogger<ProjectNewsModel> _logger = logger;

	public int id { get; set; }

	public void OnGet(int? id)
	{
		this.id = id - 1 ?? 0;
	}
}