using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant;

public class IndividualTrainingModel : PageModel
{
	private readonly ILogger<IndividualTrainingModel> _logger;

	public IndividualTrainingModel(ILogger<IndividualTrainingModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}