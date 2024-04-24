using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.CMK
{
    public class NewsModel : PageModel
    {
		public string? name { get; set; }
		public int id { get; set; }

		public void OnGet(string? name, int id)
		{
			this.name = name;
			this.id = id;
		}
	}
}
