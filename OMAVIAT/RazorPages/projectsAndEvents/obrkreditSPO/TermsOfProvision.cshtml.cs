using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.obrkreditSPO;

public class TermsOfProvisionModel : PageModel
{
	private readonly ILogger<TermsOfProvisionModel> _logger;

	public TermsOfProvisionModel(ILogger<TermsOfProvisionModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}