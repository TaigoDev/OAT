using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using RepoDb;

namespace OAT.Controllers
{
    public class MysqlController : Controller
    {
        [HttpPost, Route("api/mysql/cmd"), AuthorizeRoles(Enums.Role.www_admin), NoCache]
        public async Task<IActionResult> SendCmd(string command)
        {
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
            Logger.Info($"Пользователь {User.GetUsername()} выполним команду в базе данных - {command}\nIP: {HttpContext.UserIP()}");
            return Ok(answer);
        }
    }
}
