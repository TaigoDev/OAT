using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.obrkreditSPO;

public class MaterialsForTeachersModel : PageModel
{
	private readonly ILogger<MaterialsForTeachersModel> _logger;

	public MaterialsForTeachersModel(ILogger<MaterialsForTeachersModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}