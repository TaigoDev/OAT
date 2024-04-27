using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.admin
{
	[NoCache, Authorize]
	public class MySQLModel : PageModel
	{
		public async void OnGet()
		{

		}
	}
}
