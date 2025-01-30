using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.another.museum;

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