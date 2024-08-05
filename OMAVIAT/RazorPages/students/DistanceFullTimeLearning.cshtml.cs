using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.students
{
	public class DistanceFullTimeLearningModel : PageModel
	{
		private readonly ILogger<DistanceFullTimeLearningModel> _logger;

		public DistanceFullTimeLearningModel(ILogger<DistanceFullTimeLearningModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{

		}
	}
}