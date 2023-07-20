using Microsoft.AspNetCore.Mvc.RazorPages;
using MySqlConnector;
using Recovery.Tables;
using RepoDb;

namespace OAT.Pages.admin
{
    [AuthorizeRoles(Enums.Role.admin)]
    public class UsersModel : PageModel
    {
        public List<users> users = new List<users>();

        public async void OnGet()
        {
            if (!HttpContext.Request.Query["oq"].Any())
                Redirect($"admin/users?oq={Utils.RandomString(64)}");
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            users = (await connection.QueryAllAsync<users>()).ToList();
        }
    }
}
