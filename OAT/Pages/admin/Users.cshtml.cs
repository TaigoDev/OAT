using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using Recovery.Tables;
using RepoDb;

namespace OAT.Pages.admin
{
    [AuthorizeRoles(Enums.Role.www_admin)]
    public class UsersModel : PageModel
    {

        public async void OnGet()
        {
        }
    }
}
