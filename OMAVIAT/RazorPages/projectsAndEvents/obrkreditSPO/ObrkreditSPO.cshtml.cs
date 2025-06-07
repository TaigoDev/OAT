using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.obrkreditSPO;

public class ObrkreditSPOModel : PageModel
{
	private readonly ILogger<ObrkreditSPOModel> _logger;

	public ObrkreditSPOModel(ILogger<ObrkreditSPOModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}