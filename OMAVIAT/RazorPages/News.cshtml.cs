using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages {
	public class NewsModel : PageModel {
		public string? id { get; set; }
		public void OnGet(string? id)
		{
			this.id = id;

		}
	}
}
