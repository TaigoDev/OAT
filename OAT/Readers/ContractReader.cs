using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;

namespace OAT.Readers
{
    public class ContractReader
    {
        protected static List<Contract> contracts = new List<Contract>();

        public static async Task init()
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "kontra.csv");
            if (!File.Exists(path))
            {
                Logger.Warning("Файл kontra.csv не найден!");
                return;
            }

            var badRecord = new List<string>();
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) 
            { 
                Delimiter = ";",
                Mode = CsvMode.NoEscape,
                BadDataFound =  context => badRecord.Add(context.RawRecord)
            };

            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);
            while (csv.Read())
            {
                try
                {
                    var record = csv.GetRecord<Contract>();
                    contracts.Add(record!);
                }
                catch(Exception ex)
                {
                    Logger.ErrorWithCatch(ex.ToString());
                }
            }

            if (badRecord.Any())
            {
                var errorString = $"⚠️ При чтении файла kontra.csv произошли ошибки, которые не позволили загрузить {badRecord.Count()} строк(-у,-и)" +
                    $"\nВсего загружено строк: {badRecord.Count()}" +
                    "\nСтроки, которые не удалось загрузить:\n";
                foreach (var record in badRecord)
                    errorString += record;
                Logger.Warning(errorString);
            }
            
        }

        public static bool IsContract(Func<Contract, bool> predicate) =>
            contracts.Where(predicate).Any();

        public static bool GetContract(Func<Contract, bool> predicate, out Contract contract) 
        {
            var records = contracts.Where(predicate);
            if (records.Any())
            {
                contract = records.First();
                return true;
            }
            else
            {
                contract = null;
                return false;
            }
        }
    }
}
