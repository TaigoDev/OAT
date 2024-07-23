using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.timetable
{
	public class TeachersModel : PageModel
	{
		public string building;
		public void OnGet(string building)
		{
			this.building = building;

		}
	}
}
