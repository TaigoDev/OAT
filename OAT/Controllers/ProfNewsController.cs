using Microsoft.AspNetCore.Mvc;
using MySqlConnector;
using OAT.Readers;
using RepoDb;

namespace OAT.Controllers
{
    public class ProfNewsController : Controller
    {
        [RequestSizeLimit(1000000000000), HttpPost, Route("api/prof/news/upload"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_prof_news), NoCache]
        public async Task<IActionResult> AddFile(string title, string date, string text, List<IFormFile> files)
        {
            try
            {

                if (!Check(title, date, text))
                    return StatusCode(StatusCodes.Status400BadRequest);

                var photos = new List<string>();
                foreach (IFormFile file in files)
                    if (file.Length > 0)
                    {
                        var path = $"images/news/{Utils.sha256_hash($"{file.FileName}-{file.Length}")}{Path.GetExtension(file.FileName)}";
                        photos.Add(path);
                        using Stream fileStream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }

                using var connection = new MySqlConnection(Utils.GetConnectionString());
                var news = new ProfNews(date, title, text, text.GetWords(15), photos);
                await connection.InsertAsync(news);

                Logger.Info($"Пользователь опубликовал новую новость (Prof).\n" +
                    $"ID: {news.id}\n" +
                    $"SHA256 (TEXT): {Utils.sha256_hash(text)}\n" +
                    $"Пользователь: {User.GetUsername()}\n" +
                    $"IP-адрес: {HttpContext.UserIP()}");

                ProfNewsReader.init();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return StatusCode(StatusCodes.Status200OK);
            }
        }

        [HttpDelete("api/prof/news/{id:int}/delete"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_prof_news), NoCache]
        public async Task<IActionResult> RemoveNews(int id)
        {
            using var connection = new MySqlConnection(Utils.GetConnectionString());

            var record = await connection.QueryAsync<ProfNews>(e => e.id == id);

            if (!record.Any())
                return StatusCode(StatusCodes.Status204NoContent);
            ProfNewsReader.init();
            await connection.DeleteAsync(record.First());

            Logger.Info($"Пользователь удалил новость. (Prof)\n" +
                $"ID: {id}\n" +
                $"Пользователь: {User.Identities.ToList()[0].Claims.ToList()[0].Value}\n" +
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
