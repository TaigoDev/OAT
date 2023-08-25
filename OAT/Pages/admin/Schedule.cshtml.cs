using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.admin
{
    [AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_manager_schedule_ALL)]
    public class ScheduleModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}
