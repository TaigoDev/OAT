using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.admin
{
    [AuthorizeRoles(Enums.Role.admin)]
    public class UsersModel : PageModel
    {
        public void OnGet()
        {
            if (!HttpContext.Request.Query["oq"].Any())
                Redirect($"admin/users?oq={Utils.RandomString(64)}");
        }
    }
}
