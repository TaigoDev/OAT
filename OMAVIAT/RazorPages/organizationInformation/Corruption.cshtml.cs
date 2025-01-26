using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation;

public class CorruptionModel : PageModel
{
	private readonly ILogger<CorruptionModel> _logger;

	public CorruptionModel(ILogger<CorruptionModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}