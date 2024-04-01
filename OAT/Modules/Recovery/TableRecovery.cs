#pragma warning disable CS8600
using MySqlConnector;
using OAT.Utilities;
using RepoDb;
using System.Reflection;

namespace OAT.Modules.Recovery
{
	public class TableRecovery
	{
		public static async void Recreate(object table)
		{
			try
			{
				Type myType = table.GetType();
				IList<PropertyInfo> props = new List<PropertyInfo>(myType.GetProperties());
				string cmd = $"CREATE TABLE {table.GetType().Name} (";
				string primary_key = string.Empty;
				foreach (PropertyInfo prop in props)
				{
					if (prop.PropertyType == typeof(string) || prop.PropertyType == typeof(Enums.Role))
						cmd += $"{prop.Name} text(10000), ";
					else if (prop.PropertyType == typeof(int))
					{
						cmd += $"{prop.Name} int(255), ";
						if (prop.Name == "id")
							primary_key = prop.Name;
					}
					else if (prop.PropertyType == typeof(bool))
						cmd += $"{prop.Name} boolean, ";
					else if (prop.PropertyType == typeof(DateTime))
						cmd += $"{prop.Name} DATETIME, ";
				}
				cmd = primary_key != string.Empty ? cmd + $"PRIMARY KEY ({primary_key}));" : $"{cmd.Remove(cmd.Length - 2)});";
				using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
				await connection.ExecuteNonQueryAsync(cmd);
				Logger.InfoWithoutTelegram($"Recovery.Tables: We have successfully restored the {table.GetType().Name} table");
			}
			catch (Exception ex)
			{
				Logger.Error($"FAIL: We were unable to restore the tables Ex: {ex}");
			}
		}
	}

}
