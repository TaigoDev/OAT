using System.Text;

namespace OMAVIAT.Controllers.Security
{
	public class Permissions
	{

		public static List<Role> GetUserRoles(string username)
		{
			var results = Ldap.SearchByUsername(username);
			var roles = new List<Role>();
			var attribute = Ldap.GetAttributeByTag(results, "memberOf");

			if (attribute is null)
				return [];

			var values = attribute!.GetValues(typeof(byte[]));
			for (int id = 0; id < values.Length; id++)
			{
				var RoleName = Encoding.UTF8.GetString((values[id] as byte[])!).Split(',')[0].Replace("CN=", "");
				var success = Enum.TryParse(RoleName, out Role Role);
				if (!success)
					continue;
				roles.Add(Role);
			}
			return roles;
		}


		public static bool RightsToBuildingById(string username, string BuildingName)
		{
			var roles = GetUserRoles(username);
			if (roles is null)
				return false;

			if (roles.Any(e => e == Role.www_admin))
				return true;

			foreach (var role in roles)
			{
				var stringArray = role.ToString().Split('_');
				var campusId = stringArray[^1].ToInt32();

				if (stringArray[^2] != "campus")
					continue;

				var HavePermission = BuildingName switch
				{
					"b1" => campusId == 1,
					"b2" => campusId == 2,
					"b3" => campusId == 3,
					"b4" => campusId == 4,
					_ => false
				};
				if (HavePermission)
					return true;
			}

			return false;

		}

	}



}
