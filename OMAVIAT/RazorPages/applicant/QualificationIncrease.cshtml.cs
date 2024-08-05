using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.applicant
{
	public class QualificationIncreaseModel : PageModel
	{
		private readonly ILogger<QualificationIncreaseModel> _logger;

		public QualificationIncreaseModel(ILogger<QualificationIncreaseModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}