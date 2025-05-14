using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents;

public class AviaStartSaturnModel : PageModel
{
	private readonly ILogger<AviaStartSaturnModel> _logger;

	public AviaStartSaturnModel(ILogger<AviaStartSaturnModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}