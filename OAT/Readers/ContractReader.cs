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
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) { Delimiter = ";" };


            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, config);
            var records = csv.GetRecordsAsync<Contract>();
            contracts = await records.ToListAsync();
            for(int i = 0; i < 20; i++)
            {
                if(i < contracts.Count())
                Logger.Info($"{contracts[i].NomKontrakt} {contracts[i].DataKontrakt} {contracts[i].FullName} {contracts[i].Gruppa} {contracts[i].Zakazchik}");
            } 
        }

        public static bool IsContract(Func<Contract, bool> predicate) =>
            contracts.Where(predicate).Any();
    }
}
