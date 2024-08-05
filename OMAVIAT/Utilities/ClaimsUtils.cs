using OMAVIAT.Controllers.Security;
using System.Security.Claims;

namespace OMAVIAT.Utilities
{
	public static class ClaimsUtils
	{
		public static string GetToken(this ClaimsPrincipal User) =>
			User.Identity is ClaimsIdentity identity ? identity.Claims.First(e => e.Type == "Token").Value : "404-GetToken";

		public static string GetUsername(this ClaimsPrincipal User) =>
			User.Identity is ClaimsIdentity identity ? identity.Claims.First(e => e.Type == "username").Value : "404-GetUsername";


		public static bool IsRole(this ClaimsPrincipal User, Role role)
		{
			var roles = Permissions.GetUserRoles(User.GetUsername());

			if (role.ToString().Contains("ALL"))
				for (int i = 1; i <= 4; i++)
					if (IsRole(User, Enum.Parse<Role>(role.ToString().Replace("ALL", $"campus_{i}"))))
						return true;

			foreach (var _role in roles)
				if (_role == role || _role == Role.www_admin)
					return true;
			return false;
		}

		public static bool IsRole(this ClaimsPrincipal User, params Role[] roles)
		{
			foreach (var role in roles)
				if (User.IsRole(role))
					return true;
			return false;
		}


	}
}
