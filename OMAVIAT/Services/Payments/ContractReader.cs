using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using NLog;
using OMAVIAT.Entities;

namespace OMAVIAT.Services.Payments;

public class ContractReader
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	private static List<Contract> Contracts { get; } = [];

	public static async Task Init()
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), "kontra.csv");
		if (!File.Exists(path))
		{
			Logger.Warn("[ContractReader]: Файл kontra.csv не найден!");
			return;
		}

		var badRecord = new List<string>();
		var config = new CsvConfiguration(CultureInfo.CurrentCulture)
		{
			Delimiter = ";",
			Mode = CsvMode.NoEscape,
			BadDataFound = context => badRecord.Add(context.RawRecord)
		};

		using var reader = new StreamReader(path);
		using var csv = new CsvReader(reader, config);
		while (await csv.ReadAsync())
			try
			{
				var record = csv.GetRecord<Contract>();
				Contracts.Add(record!);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString());
			}

		if (badRecord.Count != 0)
		{
			var errorString =
				$"⚠️ При чтении файла kontra.csv произошли ошибки, которые не позволили загрузить {badRecord.Count()} строк(-у,-и)" +
				$"\nВсего загружено строк: {badRecord.Count}" +
				"\nСтроки, которые не удалось загрузить:\n";
			foreach (var record in badRecord)
				errorString += record;
			Logger.Warn(errorString);
		}
	}

	public static bool IsContract(Func<Contract, bool> predicate)
	{
		return Contracts.Any(predicate);
	}

	public static bool GetContract(Func<Contract, bool> predicate, out Contract? contract)
	{
		contract = Contracts.FirstOrDefault(predicate);
		return contract != null;
	}
}