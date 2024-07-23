using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class AllNewsModel(ILogger<AllNewsModel> logger) : PageModel
	{
		private readonly ILogger<AllNewsModel> _logger = logger;

		public int id { get; set; }
		public void OnGet(int? id)
		{

			this.id = id - 1 ?? 0;
		}

	}
}