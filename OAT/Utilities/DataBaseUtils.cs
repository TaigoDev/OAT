using MySqlConnector;

namespace OAT.Utilities
{
	public class DataBaseUtils
	{

		public static string GetConnectionString() => new MySqlConnectionStringBuilder
		{
			Server = Configurator.old_mysql.db_ip,
			UserID = Configurator.old_mysql.db_user,
			Password = Configurator.old_mysql.db_password,
			Database = Configurator.old_mysql.db_name,
			MaximumPoolSize = 2000u,
			AllowUserVariables = true
		}.ConnectionString;


	}
}
