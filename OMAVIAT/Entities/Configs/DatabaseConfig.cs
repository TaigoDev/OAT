namespace OMAVIAT.Entities.Configs;

public class DatabaseConfig
{
	public string Address { get; set; } = "mysql.ip";
	public int Port { get; set; } = 3306;
	public string Name { get; set; } = "oat";
	public string Username { get; set; } = "root";
	public string Password { get; set; } = "root";
}