using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class YearOfSPOModel : PageModel
{
	private readonly ILogger<YearOfSPOModel> _logger;

	public YearOfSPOModel(ILogger<YearOfSPOModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}