using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.projectsAndEvents.professionalitet
{
	public class NewsModel : PageModel
	{
		public string? id { get; set; }
		public void OnGet(string? id)
		{
			this.id = id;

		}
	}
}
