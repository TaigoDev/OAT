using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class RepairProgressModel : PageModel
{
	private readonly ILogger<RepairProgressModel> _logger;

	public RepairProgressModel(ILogger<RepairProgressModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}