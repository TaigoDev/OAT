using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class NewWorkshopsModel : PageModel
{
	private readonly ILogger<NewWorkshopsModel> _logger;

	public NewWorkshopsModel(ILogger<NewWorkshopsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}