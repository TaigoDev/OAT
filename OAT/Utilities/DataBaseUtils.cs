using MySqlConnector;
using RepoDb;
using RepoDb.Extensions;

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

		public static async Task<int> getLastId(string table, string parametr = "id")
		{
			try
			{
				var userId = 0;
				using var connection = new MySqlConnection(GetConnectionString());

				var obj = await connection.ExecuteQueryAsync<dynamic>($"SELECT MAX({parametr}) AS max FROM `{table}`;");
				if (obj.AsList()[0].max != null)
					userId = obj.AsList()[0].max + 1;
				return userId;
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
				return 0;
			}
		}
	}
}
