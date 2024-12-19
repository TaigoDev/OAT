using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students {
	public class AlumniEmploymentCenterOmaviatModel : PageModel {
		private readonly ILogger<AlumniEmploymentCenterOmaviatModel> _logger;

		public AlumniEmploymentCenterOmaviatModel(ILogger<AlumniEmploymentCenterOmaviatModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}
