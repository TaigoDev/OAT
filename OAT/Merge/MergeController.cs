using MySqlConnector;
using OAT.Entities.Database;
using OAT.Utilities;
using RepoDb;

namespace OAT.Merge
{
	public class MergeController
	{
		public static async Task MergeAsync()
		{
			if(File.Exists(Path.Combine(Directory.GetCurrentDirectory(), "database-merge-1.merge")))
			{
				Logger.Info("Миграция уже выполнена!");
				return;
			}
			Logger.Info("Выполняю миграцию в новую базу данных...");
			using var db = new DatabaseContext();
			using var mysql = new MySqlConnection(DataBaseUtils.GetConnectionString());
			Logger.Info("Миграция новостей главной страницы...");
			var count = 0;
			foreach(var news in await mysql.QueryAllAsync<OAT.Entities.Old.Database.News>())
			{
				await db.AddAsync(new News(news.date, news.title, news.description, news.short_description, news.photos));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей с главной страницы.");

			Logger.Info("Миграция новостей профессионалитета...");
			count = 0;
			foreach (var news in await mysql.QueryAllAsync<OAT.Entities.Old.Database.ProfNews>())
			{
				await db.AddAsync(new ProfNews(news.date, news.title, news.description, news.short_description, news.photos));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей профессионалитета.");


			Logger.Info("Миграция новостей демоэкзамена...");
			count = 0;
			foreach (var news in await mysql.QueryAllAsync<OAT.Entities.Old.Database.DemoExamNews>())
			{
				await db.AddAsync(new DemoExamNews(news.date, news.title, news.description, news.short_description, news.photos));
				count++;
			}
			await db.SaveChangesAsync();
			Logger.Info($"Мигрировало {count} новостей демоэкзамена.");
			File.CreateText(Path.Combine(Directory.GetCurrentDirectory(), "database-merge-1.merge"));
			Logger.Info("✅ Миграция успешно выполнена");
		}
	}
}
