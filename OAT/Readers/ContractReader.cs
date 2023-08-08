using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace OAT.Readers
{
    public class ContractReader
    {
        protected static List<Contract> contracts = new List<Contract>();

        public static void init()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "database.csv");
            if (!File.Exists(path))
            {
                Logger.Warning("Файл database.csv не найден!");
                return;
            }
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };


            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);
            contracts = csv.GetRecords<Contract>().ToList();
        }

        public static bool IsContract(Func<Contract, bool> predicate) =>
            contracts.Where(predicate).Any();
    }
}
