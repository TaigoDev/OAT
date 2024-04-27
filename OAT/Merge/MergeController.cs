namespace OAT.Merge
{
	public class MergeController
	{
		public static async Task MergeAsync()
		{
			Logger.Info("Выполняю миграцию в новую базу данных...");
			using var db = new DatabaseContext();

		}
	}
}
