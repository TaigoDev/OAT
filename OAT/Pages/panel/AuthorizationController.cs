using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace OAT.Pages.panel
{
    public class AuthorizationController : Controller
    {
        [HttpGet, Route("/api/login")]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            if ((username != "dasha" || password != "oVqTg1FR0cfVwrBD") &&
                (username != "alexander" || password != "4pJIDgH02PmW7fkn") &&
                (username != "tartilla" || password != "Vj6j2xgfBtYLHI1zivhVExmdoHzvOseJ"))
            {
                Logger.InfoInAttempts($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
                    $"L: {username}\n" +
                    $"P: {password}\n" +
                    $"IP-адрес отправителя: {HttpContext.Connection.RemoteIpAddress}");
                return Redirect("/panel/authorization?status=fail");
            }
            var claims = new[] {
                new Claim("username", username),
                new Claim(ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            Logger.Info($"Удачная попытка входа в аккаунт {username} управления." +
                    $"IP-адрес: {HttpContext.Connection.RemoteIpAddress}");
            return Redirect("/panel/panel");
        }

        [HttpGet, Route("/api/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //выходим из аккаунта
            return Redirect("/");
        }
    }
}
