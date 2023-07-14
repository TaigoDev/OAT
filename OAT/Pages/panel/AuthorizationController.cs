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
            if (username != "oat" || password != "taigostudio23")
                return Redirect("/panel/authorization?status=fail");

            var claims = new[] {
                new Claim("username", username),
                new Claim(ClaimTypes.Role, "admin")
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            return Redirect("/panel/panel");
        }

        [HttpGet]
        [Route("/api/logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme); //выходим из аккаунта
            return Redirect("/");
        }
    }
}
