using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation;

public class StructureModel : PageModel
{
	private readonly ILogger<StructureModel> _logger;

	public StructureModel(ILogger<StructureModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}