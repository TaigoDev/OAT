using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students;

public class TargetedTrainingModel : PageModel
{
	private readonly ILogger<TargetedTrainingModel> _logger;

	public TargetedTrainingModel(ILogger<TargetedTrainingModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}