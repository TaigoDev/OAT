using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
	public class ProjectNewsModel(ILogger<ProjectNewsModel> logger) : PageModel
	{

		public int id { get; set; }
		public void OnGet(int? id)
		{
			this.id = id - 1 ?? 0;
		}
	}
}