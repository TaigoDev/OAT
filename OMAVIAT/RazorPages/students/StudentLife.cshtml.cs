using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students
{
	public class StudentLifeModel : PageModel
	{
		private readonly ILogger<StudentLifeModel> _logger;

		public StudentLifeModel(ILogger<StudentLifeModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}