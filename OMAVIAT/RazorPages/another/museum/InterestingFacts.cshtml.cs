using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

public class InterestingFactsModel : PageModel
{
	private readonly ILogger<InterestingFactsModel> _logger;

	public InterestingFactsModel(ILogger<InterestingFactsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}