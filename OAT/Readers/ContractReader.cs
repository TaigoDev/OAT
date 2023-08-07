using CsvHelper;
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
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            contracts = csv.GetRecords<Contract>().ToList();
        }

        public static bool IsContract(Func<Contract, bool> predicate) => 
            contracts.Where(predicate).Any();
    }
}
