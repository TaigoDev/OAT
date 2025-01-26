using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class EiSRModel : PageModel
{
	private readonly ILogger<EiSRModel> _logger;

	public EiSRModel(ILogger<EiSRModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}