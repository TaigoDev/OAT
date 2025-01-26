namespace OMAVIAT.Entities.Configs;

public class LdapConfig
{
	public string Address { get; set; } = "ldap.ip";
	public int Port { get; set; } = 389;
	public string Username { get; set; } = "root";
	public string Password { get; set; } = "password";
	public string Domain { get; set; } = "site";
	public string Zone { get; set; } = "zone";
}