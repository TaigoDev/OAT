using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.RazorPages.timetable {
	public class CabinetsModel : PageModel {
		public string? building;
		public void OnGet(string building)
		{
			this.building = building;

		}
	}
}
