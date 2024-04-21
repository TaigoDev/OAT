using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class CMKModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public CMKModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
		public string? name { get ; set; }	
		public void OnGet(string? name)
		{
			this.name = name;

		}
	}
}