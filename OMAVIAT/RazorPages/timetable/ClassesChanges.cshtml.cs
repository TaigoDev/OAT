using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.timetable;

[NoCache]
public class ClassesChangesModel : PageModel
{
	private readonly ILogger<ClassesChangesModel> _logger;

	public ClassesChangesModel(ILogger<ClassesChangesModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}