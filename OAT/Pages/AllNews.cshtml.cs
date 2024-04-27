using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class AllNewsModel(ILogger<IndexModel> logger) : PageModel
	{
		private readonly ILogger<IndexModel> _logger = logger;

		public int id { get; set; }
		public void OnGet(int? id)
		{

			this.id = id - 1 ?? 0;
		}

	}
}