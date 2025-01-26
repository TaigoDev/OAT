using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

public class OPOPPModel : PageModel
{
	private readonly ILogger<OPOPPModel> _logger;

	public OPOPPModel(ILogger<OPOPPModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}