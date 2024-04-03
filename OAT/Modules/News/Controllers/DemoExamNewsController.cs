using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using OAT.Entities.Database;
using OAT.Modules.MNews.Readers;
using OAT.Utilities;
using RepoDb;

namespace OAT.Modules.MNews.Controllers
{
	public class DemoExamNewsController : Controller
	{
		[RequestSizeLimit(1000000000000), HttpPost, Route("api/demo/exam/news/upload"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_prof_news), NoCache]
		public async Task<IActionResult> AddFile(string title, string date, string text, List<IFormFile> files)
		{
			try
			{

				if (!Check(title, date, text) || !files.Any())
					return StatusCode(StatusCodes.Status400BadRequest);

				var photos = new List<string>();
				foreach (IFormFile file in files)
					if (file.Length > 0)
					{
						var path = $"images/news/{StringUtils.SHA226($"{file.FileName}-{file.Length}")}{Path.GetExtension(file.FileName)}";
						photos.Add(path);
						using Stream fileStream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
						await file.CopyToAsync(fileStream);
					}

				using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
				var news = new DemoExamNews(date, title, text, text.GetWords(15), photos);
				await connection.InsertAsync(news);

				Logger.Info($"Пользователь опубликовал новую новость (DemoExam).\n" +
					$"ID: {news.id}\n" +
					$"SHA256 (TEXT): {StringUtils.SHA226(text)}\n" +
					$"Пользователь: {User.GetUsername()}\n" +
					$"IP-адрес: {HttpContext.UserIP()}");

				await ProfNewsReader.init();
				return StatusCode(StatusCodes.Status200OK);
			}
			catch (Exception ex)
			{
				Logger.Error(ex.ToString());
				return StatusCode(StatusCodes.Status200OK);
			}
		}

		[HttpDelete("api/demo/exam/news/{id:int}/delete"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_prof_news), NoCache]
		public async Task<IActionResult> RemoveNews(int id)
		{
			using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());

			var record = await connection.QueryAsync<ProfNews>(e => e.id == id);

			if (!record.Any())
				return StatusCode(StatusCodes.Status204NoContent);
			await ProfNewsReader.init();
			await connection.DeleteAsync(record.First());

			Logger.Info($"Пользователь удалил новость. (DemoExam)\n" +
				$"ID: {id}\n" +
				$"Пользователь: {User.GetUsername()}\n" +
				$"IP-адрес: {HttpContext.UserIP()}");
			return StatusCode(StatusCodes.Status200OK);
		}

		private bool Check(params string[] strings)
		{
			foreach (var s in strings)
				if (string.IsNullOrWhiteSpace(s))
					return false;
			return true;
		}
	}
}
