using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages;

[NoCache]
[Authorize]
public class PanelModel : PageModel
{
	private readonly ILogger<PanelModel> _logger;

	public PanelModel(ILogger<PanelModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}