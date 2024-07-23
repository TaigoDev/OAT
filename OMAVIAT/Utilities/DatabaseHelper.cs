using Microsoft.EntityFrameworkCore;

namespace OAT.Utilities
{
	public class DatabaseHelper
	{
		public static async Task WaitStableConnection()
		{

			using var context = new DatabaseContext();
			Logger.Info("DatabaseContext: Ожидание подключения к базе данных...");
			int attempt = 0;
			while (true)
			{
				try
				{
					await context.Database.OpenConnectionAsync();
					await context.Database.CloseConnectionAsync();
					Logger.Info($"✅ Подключение с базой данных успешно установлено!");
					return;
				}
				catch (Exception ex)
				{
					attempt++;
					if (attempt is not 1)
						Logger.Error($"DatabaseContext: Не удалось подключиться к базе данных. Ожидаю успешного подключения... \nОшибка: {ex}");
					await Task.Delay(3000);

				}
			}
		}
	}
}
