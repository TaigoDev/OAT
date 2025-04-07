using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.distanceLearning.Main;

public class MainModel : PageModel
{
	private readonly ILogger<MainModel> _logger;

	public MainModel(ILogger<MainModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}