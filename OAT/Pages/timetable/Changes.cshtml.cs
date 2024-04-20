using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Controllers.Schedules.Controllers;

namespace OAT.Pages
{
	[NoCache]
	public class ChangesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public ChangesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public string? sheet { get; set; }
		public int? corpus { get; set; }
		public string captchaToken { get; set; }

		public void OnGet(int? corpus, string? sheet)
		{

			if (corpus is null)
				return;


			this.corpus = corpus;
			this.sheet = sheet ?? ChangesHelper.GetSheetName((int)corpus);
		}
	}

}