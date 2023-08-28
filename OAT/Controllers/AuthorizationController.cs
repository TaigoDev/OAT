using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using OAT.Utilities;
using RepoDb;
using System.DirectoryServices.Protocols;
using System.Net;
using System.Security.Claims;
using YamlDotNet.Core.Tokens;
using static Enums;

namespace OAT.Controllers
{
    public class AuthorizationController : Controller
    {
        [HttpGet, Route("/api/login"), NoCache]
        public async Task<IActionResult> Login([FromQuery] string username, [FromQuery] string password)
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var IsValid = LdapValidateCredentials(username, password);

            if (!IsValid)
            {
                Logger.InfoInAttempts($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
                    $"L: {username}\n" +
                    $"IP-адрес отправителя: {HttpContext.UserIP()}");
                return Redirect("/admin/authorization?status=fail");
            }

            ClearExpiredTokens(username);
            var Token = Utils.RandomString(450);
            await connection.InsertAsync(new Tokens(username, Token, DateTime.UtcNow.ToString("dd.MM.yyyy HH:mm:ss")));


            var claims = new List<Claim>() {
                new Claim("username", username),
                new Claim("Token", Token),

            };
            var roles = Permissions.GetUserRoles(username);
            foreach (var role in roles)
                claims.Add(new Claim(ClaimTypes.Role, role.ToString()));

            var identity = new ClaimsIdentity(claims.ToArray(), CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            Logger.Info($"Удачная попытка входа в аккаунт {username} управления. " +
                    $"IP-адрес: {HttpContext.UserIP()}");
            return Redirect($"/admin/panel");
        }

        [HttpGet, Route("/api/logout"), NoCache]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect("/");
        }

        public static async Task<Enums.AuthResult> ValidateCredentials(ClaimsPrincipal user, string IP)
        {
            try
            {
                var Token = user.GetToken();
                using var connection = new MySqlConnection(Utils.GetConnectionString());

                var records = await connection.QueryAsync<Tokens>(e => e.Token == Token);
                if (!records.Any())
                    return AuthResult.fail;

                var IsSuccess = false;
                foreach (var record in records)
                {
                    if (record.username != user.GetUsername() || DateTime.ParseExact(record.issued, "dd.MM.yyyy HH:mm:ss", null).AddMinutes(30) < DateTime.UtcNow)
                        continue;
                    IsSuccess = true;
                }

                return IsSuccess ? AuthResult.success : AuthResult.token_expired;
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return AuthResult.token_expired;
            }
        }

        private static bool LdapValidateCredentials(string username, string password)
        {

            try
            {
                var authType = OperatingSystem.IsWindows() ? AuthType.Negotiate : AuthType.Basic;
                username = OperatingSystem.IsWindows() ? username : $"{ProxyController.config.ldap_domain}\\{username}";
                var conn = new LdapConnection(new LdapDirectoryIdentifier(ProxyController.config.ldap_IP, ProxyController.config.ldap_port), new NetworkCredential(username, password), authType);
                conn.Bind();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private static async void ClearExpiredTokens(string username)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var records = await connection.QueryAsync<Tokens>(e => e.username == username);
            foreach (var record in records)
                if (DateTime.ParseExact(record.issued, "dd.MM.yyyy HH:mm:ss", null).AddMinutes(30) < DateTime.UtcNow)
                    await connection.DeleteAsync(record);
        }
    }
}
