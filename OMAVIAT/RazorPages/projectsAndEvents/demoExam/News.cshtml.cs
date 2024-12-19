using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.projectsAndEvents.demoExam {
	public class NewsModel : PageModel {
		public string? id { get; set; }
		public void OnGet(string? id)
		{
			this.id = id;
		}
	}
}
