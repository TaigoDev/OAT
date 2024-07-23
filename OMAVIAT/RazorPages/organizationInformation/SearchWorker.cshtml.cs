using Microsoft.AspNetCore.Mvc.RazorPages;
using static OAT.Controllers.Workers.WorkersReader;

namespace OAT.Pages.organizationInformation
{
	public class SearchWorkerModel : PageModel
	{

		public List<Worker> Workers = new List<Worker>();

		public void OnGet(string? text)
		{
			if (text is null || text is "undefined")
			{
				Response.Redirect("/organizationInformation/ManagementPedagogicalStaff");
				return;
			}

			if (text.Length < 3)
				return;
			Workers = AllWorkers.Where(e => e.FullName.ToLower().Contains(text.ToLower())).ToList();
		}
	}
}
