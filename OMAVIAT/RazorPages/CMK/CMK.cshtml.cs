using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.CMK
{
	public class CMKModel : PageModel
	{
		private readonly ILogger<CMKModel> _logger;

		public CMKModel(ILogger<CMKModel> logger)
		{
			_logger = logger;
		}
		public string? name { get; set; }
		public void OnGet(string? name)
		{
			this.name = name;

		}
	}
}