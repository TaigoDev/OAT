using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.applicant
{
	public class SpecialtiesPartTimeEveningModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public SpecialtiesPartTimeEveningModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}