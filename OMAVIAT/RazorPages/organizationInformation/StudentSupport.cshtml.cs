using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.organizationInformation
{
	public class StudentSupportModel : PageModel
	{
		private readonly ILogger<StudentSupportModel> _logger;

		public StudentSupportModel(ILogger<StudentSupportModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}