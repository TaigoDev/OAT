using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.admin
{
    [AuthorizeRoles(Enums.Role.admin, Enums.Role.reporter, Enums.Role.manager)]
    public class ScheduleModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
