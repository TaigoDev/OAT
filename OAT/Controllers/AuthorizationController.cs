using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using Newtonsoft.Json;
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
                Logger.Info($"Неудачная попытка входа в аккаунт управления. Используемые данные:\n" +
                    $"L: {username}\n" +
                    $"IP-адрес отправителя: {HttpContext.UserIP()}");
                return Redirect("/admin/authorization?status=fail");
            }

            ClearExpiredTokens(username);
            var Token = Utils.RandomString(450);
            var roles = Permissions.GetUserRoles(username);
            if(roles.Count is 0 || roles is null)
            {
                Logger.Info($"Запрос на авторизацию через аккаунт {username} отклонен, т.к. пользователь не имеет ни одного права связанного сайтом.\n IP: {HttpContext.UserIP()}");
                return Redirect("/admin/authorization?status=fail");
            }
            await connection.InsertAsync(new Tokens(username, Token, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), JsonConvert.SerializeObject(roles, new Newtonsoft.Json.Converters.StringEnumConverter())));

            var claims = new List<Claim>() {
                new Claim("username", username),
                new Claim("Token", Token),
            };
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
            return Redirect($"/admin/authorization{HttpContext.Request.QueryString}");
        }

        public static async Task<AuthResult> ValidateCredentials(ClaimsPrincipal user, string IP)
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
                    if (record.username != user.GetUsername() || DateTime.ParseExact(record.issued, "dd.MM.yyyy HH:mm:ss", null).AddMinutes(30) < DateTime.Now)
                        continue;

                    if (user.GetToken() == record.Token)
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
                Logger.Error(ex.ToString());
                return false;
            }
        }

        private static async void ClearExpiredTokens(string username)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());
            var records = await connection.QueryAsync<Tokens>(e => e.username == username);
            foreach (var record in records)
                if (DateTime.ParseExact(record.issued, "dd.MM.yyyy HH:mm:ss", null).AddMinutes(30) < DateTime.Now)
                    await connection.DeleteAsync(record);
        }
    }
}
