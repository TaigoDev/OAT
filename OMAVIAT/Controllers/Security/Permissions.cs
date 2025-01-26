using System.Text;

namespace OMAVIAT.Controllers.Security;

public class Permissions
{
	public static List<Role> GetUserRoles(string username)
	{
		if (!Configurator.Config.IsProduction)
			return [Role.www_admin];
		var results = Ldap.SearchByUsername(username);
		var roles = new List<Role>();
		var attribute = Ldap.GetAttributeByTag(results, "memberOf");

		if (attribute is null)
			return [];

		var values = attribute.GetValues(typeof(byte[]));
		foreach (var t in values)
		{
			var roleName = Encoding.UTF8.GetString((t as byte[])!).Split(',')[0].Replace("CN=", "");
			var success = Enum.TryParse(roleName, out Role role);
			if (!success)
				continue;
			roles.Add(role);
		}

		return roles;
	}


	public static bool RightsToBuildingById(string username, string buildingName)
	{
		if (!Configurator.Config.IsProduction)
			return true;

		var roles = GetUserRoles(username);

		if (roles.Any(e => e == Role.www_admin))
			return true;

		foreach (var role in roles)
		{
			var stringArray = role.ToString().Split('_');
			var campusId = stringArray[^1].ToInt32();

			if (stringArray[^2] != "campus")
				continue;

			var havePermission = buildingName switch
			{
				"b1" => campusId == 1,
				"b2" => campusId == 2,
				"b3" => campusId == 3,
				"b4" => campusId == 4,
				_ => false
			};
			if (havePermission)
				return true;
		}

		return false;
	}
}