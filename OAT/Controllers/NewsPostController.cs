using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace OAT.Controllers
{
    public class NewsPostController : Controller
    {
        [HttpPost, Route("api/news/upload"), Authorize]
        public async Task<IActionResult> AddFile(string title, string date, string text, List<IFormFile> files)
        {
            var photos = new List<string>();
            foreach (IFormFile file in files)
                if (file.Length > 0)
                {
                    var path = $"images/news/{Utils.sha256_hash($"{file.FileName}-{file.Length}")}{Path.GetExtension(file.FileName)}";
                    photos.Add(path);
                    using Stream fileStream = new FileStream(Path.Combine("wwwroot", path), FileMode.Create);
                    await file.CopyToAsync(fileStream);
                }

            System.IO.File.WriteAllText($"news/{NewsController.News.Count()}.yaml", new NewsFile(date, title, text, photos).SerializeYML());

            Logger.Info($"Пользователь опубликовал новую новость.\n" +
                $"Текст: {text}\n" +
                $"Дата: {date}\n" +
                $"Имя: {title}\n" +
                $"Пользователь: {User.Identities.ToList()[0].Claims.ToList()[0].Value}\n" +
                $"IP-адрес: {HttpContext.Connection.RemoteIpAddress}");
            return Redirect("/");
        }


    }
}
