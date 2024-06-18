using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Utilities;

namespace OAT.Pages.CMK
{
	public class CMKModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public CMKModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}
		public string? name { get; set; }
		public void OnGet(string? name)
		{
			this.name = Translit.TranslitEnToRus(name);
		}
	}
}