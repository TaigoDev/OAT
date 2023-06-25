using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace OAT.Controllers
{
    public class NewsController : Controller
    {
        [HttpPost]
        public async Task<IActionResult> AddFile(IFormFile uploadedFile)
        {
            if (uploadedFile != null)
                using (var fileStream = new FileStream($"wwwroot/news/{sha256_hash($"{uploadedFile.Name}-{uploadedFile.Length}.png")}", FileMode.Create))
                    await uploadedFile.CopyToAsync(fileStream);
                
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
