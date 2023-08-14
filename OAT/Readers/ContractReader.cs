using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using static System.Net.Mime.MediaTypeNames;

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
            var config = new CsvConfiguration(CultureInfo.CurrentCulture) 
            { 
                Delimiter = ";",
                BadDataFound = null
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
                    Logger.Error($"Ошибка загрузки договора: {ex}");
                }
            }

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
