using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OAT.function;
using System.Security.Cryptography;
using System.Text;

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
                    photos.Add($"images/news/{sha256_hash($"{file.FileName}-{file.Length}")}.png");
                    using (Stream fileStream = new FileStream(Path.Combine("wwwroot/images/news", sha256_hash($"{file.FileName}-{file.Length}") + ".png"), FileMode.Create))
                        await file.CopyToAsync(fileStream);
                }

            System.IO.File.WriteAllText($"news/{function.NewsController.News.Count()}.yaml", new function.NewsController.NewsDataYML(date, title, text, photos).SerializeYML());

            return Redirect("/");
        }

        public static String sha256_hash(string value)
        {
            StringBuilder Sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(value));

                foreach (byte b in result)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();
        }
    }
}
