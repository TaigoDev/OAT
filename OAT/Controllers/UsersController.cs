using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Recovery.Tables;
using RepoDb;

namespace OAT.Controllers
{
    public class UsersController : Controller
    {
        [HttpPost, Route("api/users/new"), AuthorizeRoles(Enums.Role.admin)]
        public async Task<IActionResult> NewUser(string Fullname, string username, string password, string role)
        {
            try
            {
                if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                    return StatusCode(StatusCodes.Status401Unauthorized);
                using var connection = new MySqlConnection(Utils.GetConnectionString());
                if (string.IsNullOrWhiteSpace(Fullname) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(role))
                    return StatusCode(StatusCodes.Status406NotAcceptable);

                if (role == "Репортер")
                    role = Enums.Role.reporter.ToString();
                else if (role == "Администратор")
                    role = Enums.Role.admin.ToString();
                else
                    role = Enums.Role.manager.ToString();

                await connection.InsertAsync(new users(Fullname, username, Utils.sha256_hash(password), role));

                return StatusCode(StatusCodes.Status200OK);
            }
            catch(Exception ex) {
                Console.WriteLine(ex);
                return Ok(ex);
            }
        }
    }
}
