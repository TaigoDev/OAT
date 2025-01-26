using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another;

public class CityImprovementModel : PageModel
{
	private readonly ILogger<CityImprovementModel> _logger;

	public CityImprovementModel(ILogger<CityImprovementModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}