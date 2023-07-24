using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using RepoDb;

namespace OAT.Controllers
{
    public class MysqlController : Controller
    {
        [HttpPost, Route("api/mysql/cmd"), AuthorizeRoles(Enums.Role.admin)]
        public async Task<IActionResult> SendCmd(string command)
        {
            if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                return StatusCode(StatusCodes.Status401Unauthorized);
            var answer = new object();
            try
            {
                using var connection = new MySqlConnection(Utils.GetConnectionString());
                answer = await connection.ExecuteQueryAsync<object>(command);
            }
            catch (Exception ex)
            {
                answer = ex.Message;
            }
            Logger.Info($"Пользователь {User.Username()} выполним команду в базе данных - {command}\nIP: {HttpContext.UserIP()}");
            return Ok(answer);
        }
    }
}
