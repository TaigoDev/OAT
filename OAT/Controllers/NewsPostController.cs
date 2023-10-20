using Microsoft.AspNetCore.Mvc;
using OAT.Utilities;

namespace OAT.Controllers
{
    public class NewsPostController : Controller
    {
        [RequestSizeLimit(1000000000000), HttpPost, Route("api/news/upload"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_news), NoCache]
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
                        var path = $"images/news/{StringUtils.SHA226($"{file.FileName}-{file.Length}")}{Path.GetExtension(file.FileName)}";
                        photos.Add(path);
                        using Stream fileStream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
                        await file.CopyToAsync(fileStream);
                    }

                System.IO.File.WriteAllText($"news/{NewsReader.News.Count()}.yaml", new NewsFile(date, title, text, photos).SerializeYML());
                Logger.Info($"Пользователь опубликовал новую новость.\n" +
                    $"ID: {NewsReader.News.Count()}\n" +
                    $"SHA256 (TEXT): {StringUtils.SHA226(text)}\n" +
                    $"Пользователь: {User.GetUsername()}\n" +
                    $"IP-адрес: {HttpContext.UserIP()}");
                await NewsReader.Loader();
                return StatusCode(StatusCodes.Status200OK);
            }
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
                return StatusCode(StatusCodes.Status200OK);
            }
        }

        [HttpDelete("api/news/{id:int}/delete"), AuthorizeRoles(Enums.Role.www_admin, Enums.Role.www_reporter_news), NoCache]
        public async Task<IActionResult> RemoveNews(int id)
        {
            if (!System.IO.File.Exists($"news/{id}.yaml"))
                return StatusCode(StatusCodes.Status204NoContent);

            System.IO.File.Delete($"news/{id}.yaml");
            Logger.Info($"Пользователь удалил новость.\n" +
                $"ID: {id}\n" +
                $"Пользователь: {User.GetUsername()}\n" +
                $"IP-адрес: {HttpContext.UserIP()}");
            await NewsReader.Loader();
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
