using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.admin
{
	[NoCache, Authorize]
	public class CMKModel : PageModel
	{
		public void OnGet()
		{
		}
	}
}
