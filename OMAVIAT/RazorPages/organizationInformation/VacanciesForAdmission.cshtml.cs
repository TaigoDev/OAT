using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation;

public class VacanciesForAdmissionModel : PageModel
{
	private readonly ILogger<VacanciesForAdmissionModel> _logger;

	public VacanciesForAdmissionModel(ILogger<VacanciesForAdmissionModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
	}
}