using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OMAVIAT.Pages.admin;

[NoCache]
[Authorize]
public class CMKModel : PageModel
{
	public void OnGet()
	{
	}
}