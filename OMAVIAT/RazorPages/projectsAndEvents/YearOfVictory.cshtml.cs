using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class YearOfVictoryModel : PageModel
{
	private readonly ILogger<YearOfVictoryModel> _logger;

	public YearOfVictoryModel(ILogger<YearOfVictoryModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}