namespace OAT.Merge
{
	public class MergeController
	{
		public static async Task MergeAsync()
		{
			if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "database-merge-2.merge")))
			{
				Logger.Info("Миграция уже выполнена!");
				return;
			}
			Logger.Info("Выполняю миграцию в новую базу данных...");
			using var db = new DatabaseContext();
			using var old_db = new OldDatabaseContext();
			Logger.Info("Миграция новостей главной страницы...");
			var count = 0;
			foreach (var news in old_db.News)
			{
				await db.News.AddAsync(new Entities.Database.News(news.id, news.date, news.title, news.description, news.short_description, news.photos, false));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей с главной страницы.");

			Logger.Info("Миграция новостей профессионалитета...");
			count = 0;
			foreach (var news in old_db.ProfNews)
			{
				await db.ProfNews.AddAsync(new Entities.Database.ProfNews(news.id, news.date, news.title, news.description, news.short_description, news.photos, false));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей профессионалитета.");


			Logger.Info("Миграция новостей демоэкзамена...");
			count = 0;
			foreach (var news in old_db.DemoExamNews)
			{
				await db.DemoExamNews.AddAsync(new Entities.Database.DemoExamNews(news.id, news.date, news.title, news.description, news.short_description, news.photos, false));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей демоэкзамена.");
			File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "database-merge-2.merge"));
			Logger.Info("✅ Миграция успешно выполнена");
		}
	}
}
