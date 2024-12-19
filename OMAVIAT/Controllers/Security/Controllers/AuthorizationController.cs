using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OMAVIAT.Entities.Database;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Utilities;
using System.Security.Claims;

namespace OMAVIAT.Controllers.Security.Controllers {
	public class AuthorizationController : Controller {
		[HttpPost("api/login"), NoCache]
		public async Task<IActionResult> Login(string username, string password)
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			using var connection = new DatabaseContext();
			var IsValid = Ldap.Login(username, password, HttpContext.UserIP());

			var record = await connection.IPTables.FirstOrDefaultAsync(e => e.IP == HttpContext.UserIP());
			if (record is null)
			{
				record = new IPTables(HttpContext.UserIP() ?? "LOCAL", 0, DateTime.UtcNow.AddDays(-1), DateTime.UtcNow.AddDays(-1));
				await connection.AddAsync(record);
				await connection.SaveChangesAsync();
			}

			if (DateTime.UtcNow < record.BanTime)
				return StatusCode(StatusCodes.Status403Forbidden);

			if (!IsValid)
			{

				if (DateTime.UtcNow.AddHours(-1) > record.LastFailAttempt)
					record.attempts = 0;
				record.attempts++;
				record.LastFailAttempt = DateTime.UtcNow;
				if (record.attempts >= 3)
				{
					record.attempts = 0;
					record.BanTime = DateTime.UtcNow.AddHours(1);
				}
				connection.Update(record);
				await connection.SaveChangesAsync();
				return BadRequest();
			}

			ClearExpiredTokens(username);
			var Token = StringUtils.RandomString(450);
			var roles = Permissions.GetUserRoles(username);
			if (roles.Count is 0 || roles is null)
			{
				Logger.Info($"Запрос на авторизацию через аккаунт {username} отклонен, т.к. пользователь не имеет ни одного права связанного сайтом.\n IP: {HttpContext.UserIP()}");
				return Redirect("/admin/authorization?status=fail");
			}
			await connection.AddAsync(new Tokens(username, Token, DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"), JsonConvert.SerializeObject(roles, new Newtonsoft.Json.Converters.StringEnumConverter())));
			await connection.SaveChangesAsync();
			var claims = new List<Claim>()
			{
				new("username", username),
				new("Token", Token),
			};
			foreach (var role in roles)
				claims.Add(new(ClaimTypes.Role, role.ToString()));


			var identity = new ClaimsIdentity(claims.ToArray(), CookieAuthenticationDefaults.AuthenticationScheme);
			await HttpContext.SignInAsync(
			CookieAuthenticationDefaults.AuthenticationScheme,
			new ClaimsPrincipal(identity));

			Logger.Info($"Удачная попытка входа в аккаунт {username} управления. " +
			            $"IP-адрес: {HttpContext.UserIP()}");
			return Redirect($"/admin/panel");
		}

		[HttpPost("/api/students/login"), NoCache]
		public IActionResult LoginStudents(string username, string password)
		{
			HttpContext.SetCookie("student-username", StringUtils.Base64Encode($"{username}"));
			HttpContext.SetCookie("student-password", StringUtils.Base64Encode($"{password}"));
			return Ldap.Login(username, password, HttpContext.UserIP(), false) ? StatusCode(StatusCodes.Status200OK) : StatusCode(StatusCodes.Status401Unauthorized);
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
				using var connection = new DatabaseContext();

				var records = await connection.Tokens.Where(e => e.Token == Token).ToListAsync();
				if (records.Count == 0)
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

		private static async void ClearExpiredTokens(string username)
		{
			using var connection = new DatabaseContext();
			var records = connection.Tokens.Where(e => e.username == username);
			foreach (var record in records)
				if (DateTime.ParseExact(record.issued, "dd.MM.yyyy HH:mm:ss", null).AddMinutes(30) < DateTime.Now)
					connection.Remove(record);

			await connection.SaveChangesAsync();
		}
	}
}
