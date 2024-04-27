using Microsoft.AspNetCore.Authorization;
using static Enums;

public class AuthorizeRolesAttribute : AuthorizeAttribute
{
	public AuthorizeRolesAttribute(params Role[] allowedRoles)
	{
		var allowedRolesAsStrings = GetUnderRoles(allowedRoles.ToList()).ConvertToString();
		Roles = string.Join(",", allowedRolesAsStrings);
	}

	private List<Role> GetUnderRoles(Role role)
	{
		if (!role.ToString().Contains("ALL"))
			return new List<Role>() { role };

		var roles = new List<Role>();
		for (int i = 1; i <= campus_count; i++)
			roles.Add(Enum.Parse<Role>(role.ToString().Replace("ALL", $"campus_{i}")));
		return roles;
	}

	private List<Role> GetUnderRoles(List<Role> roles)
	{
		var _roles = new List<Role>();
		foreach (var role in roles)
			_roles.AddRange(GetUnderRoles(role));
		return _roles;
	}
}
