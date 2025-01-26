using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Models;
using static OMAVIAT.Services.Workers.WorkersReader;

namespace OMAVIAT.Pages.organizationInformation;

public class SearchWorkerModel : PageModel
{
	public List<Worker> Workers = new();

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