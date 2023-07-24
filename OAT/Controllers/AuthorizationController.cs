using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Recovery.Tables;
using RepoDb;
using System.Security.Claims;

namespace OAT.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet, Route("/api/login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var records = await connection.QueryAsync<users>((e) => e.username == username && e.password == Utils.sha256_hash(password));

            if (!records.Any())
            {
                Logger.InfoInAttempts($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
                    $"L: {username}\n" +
                    $"P: {password}\n" +
                    $"IP-адрес отправителя: {HttpContext.UserIP()}");
                return Redirect("/admin/authorization?status=fail");
            }
            var claims = new[] {
                new Claim("username", username),
                new Claim("sha256_password", Utils.sha256_hash(password)),
                new Claim(ClaimTypes.Role, records.First().role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            Logger.Info($"Удачная попытка входа в аккаунт {username} управления. Роль: {records.First().role} " +
                    $"IP-адрес: {HttpContext.UserIP()}");
            return Redirect($"/admin/panel");
        }

        [HttpGet, Route("/api/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //выходим из аккаунта
            return Redirect("/");
        }

        public static async Task<bool> CheckLogin(string username, string password)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var records = await connection.QueryAsync<users>((e) => e.username == username && e.password == password);
            return records.Any();
        }
    }
}
