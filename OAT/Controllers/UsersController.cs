using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using OAT.Utilities;
using Recovery.Tables;
using RepoDb;

namespace OAT.Controllers
{
    public class UsersController : Controller
    {
        /*
        [HttpPost, Route("api/users/new"), AuthorizeRoles(Enums.Role.admin)]
        public async Task<IActionResult> NewUser(string Fullname, string username, string password, string role, string building)
        {
            try
            {
                if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                    return StatusCode(StatusCodes.Status401Unauthorized);
                using var connection = new MySqlConnection(Utils.GetConnectionString());
                if (string.IsNullOrWhiteSpace(Fullname) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(role) ||
                    (await connection.QueryAsync<users>(e => e.username == username)).Any())
                    return StatusCode(StatusCodes.Status406NotAcceptable);

                if (role == "Репортер")
                    role = Enums.Role.reporter.ToString();
                else if (role == "Администратор")
                    role = Enums.Role.admin.ToString();
                else
                    role = Enums.Role.schedule_manager.ToString();

                await connection.InsertAsync(new users(Fullname, username, Utils.sha256_hash(password), role, building));
                Logger.Info($"Пользователь {User.Username()} добавил нового пользователя {Fullname} c ником {username} и ролью {role}");
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return Ok(ex);
            }
        }

        [HttpDelete("api/users/{id:int}/delete"), AuthorizeRoles(Enums.Role.admin)]
        public async Task<IActionResult> RemoveNews(int id)
        {
            if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                return StatusCode(StatusCodes.Status401Unauthorized);
            using var connection = new MySqlConnection(Utils.GetConnectionString());

            var records = await connection.QueryAsync<users>(e => e.id == id);
            if (!records.Any())
                return StatusCode(StatusCodes.Status204NoContent);
            await connection.DeleteAsync(records.First());
            Logger.Info($"Пользователь удалил аккаунт.\n" +
                $"Удаленный аккаунт: {id}-{records.First().username}-{records.First().FullName}\n" +
                $"Пользователь: {User.Identities.ToList()[0].Claims.ToList()[0].Value}\n" +
                $"IP-адрес: {HttpContext.UserIP()}");
            return StatusCode(StatusCodes.Status200OK);
        }
        */
    }
}
