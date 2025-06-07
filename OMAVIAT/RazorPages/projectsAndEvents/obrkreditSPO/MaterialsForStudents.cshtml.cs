using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.obrkreditSPO;

public class MaterialsForStudentsModel : PageModel
{
	private readonly ILogger<MaterialsForStudentsModel> _logger;

	public MaterialsForStudentsModel(ILogger<MaterialsForStudentsModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}