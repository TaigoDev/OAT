using MySqlConnector;
using RepoDb;
using System.Data;
using System.Reflection;
using TAIGO.ZCore.DPC.Recovery;

namespace TAIGO.ZCore.DPC.Services
{
    public class HealthTables
    {
        public static List<object> tables = new List<object>();

        public static void init() => new Task(async () =>
        {
            try
            {
                foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t =>
                t.Namespace == "Recovery.Tables").ToList())
                    tables.Add(Activator.CreateInstance(type) ?? throw new Exception("Recovery is not available"));

                using var connection = new MySqlConnection(Utils.GetConnectionString());

                var mysql_tables = (await connection.ExecuteQueryAsync<string>($"SHOW TABLES;")).ToList();
                foreach (var table in tables)
                    if (mysql_tables.Where(x => x == table.GetType().Name).FirstOrDefault() == null)
                    {
                        TableRecovery.Recreate(table);
                        Logger.Info($"HealthTables: We have successfully restored the {new List<PropertyInfo>(table.GetType().GetProperties()).GetType().Name} table");
                    }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }).Start();
    }
}
