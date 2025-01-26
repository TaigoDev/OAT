using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students;

public class AlumniEmploymentCenterOmskModel : PageModel
{
	private readonly ILogger<AlumniEmploymentCenterOmskModel> _logger;

	public AlumniEmploymentCenterOmskModel(ILogger<AlumniEmploymentCenterOmskModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}