using Microsoft.EntityFrameworkCore;
using NLog;

namespace OMAVIAT.Utilities;

public class DatabaseHelper
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task WaitStableConnection()
	{
		await using var context = new DatabaseContext();
		Logger.Info("DatabaseContext: Ожидание подключения к базе данных...");
		var attempt = 0;
		while (true)
			try
			{
				await context.Database.OpenConnectionAsync();
				await context.Database.CloseConnectionAsync();
				Logger.Info("\u2705 Подключение с базой данных успешно установлено!");
				await using var db = new DatabaseContext();
				Console.WriteLine($"Количество новостей: {db.News.Count()} ");
				//await db.Database.MigrateAsync();
				await DropTokens();
				return;
			}
			catch (Exception ex)
			{
				attempt++;
				if (attempt is not 1)
					Logger.Error(
						$"DatabaseContext: Не удалось подключиться к базе данных. Ожидаю успешного подключения... \nОшибка: {ex}");
				await Task.Delay(3000);
			}
	}

	private static async Task DropTokens()
	{
		await using var connection = new DatabaseContext();
		Console.WriteLine($"{await connection.Tokens.CountAsync()}");
		connection.RemoveRange(connection.Tokens);
		await connection.SaveChangesAsync();
	}
}