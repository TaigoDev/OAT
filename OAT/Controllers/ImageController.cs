using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OAT.function;
using System.Net;
using System.Net.Mime;

namespace OAT.Controllers
{
    public class ImageController : Controller
    {
        [HttpGet, Route("proxing/images/bitrix")]
        public async Task<IActionResult> getImage([FromQuery] string url)
        {
            var contentType = string.Empty;
            new FileExtensionContentTypeProvider().TryGetContentType(url, out contentType);
            var local_path = $"bitrix/{Utils.GetSHA256(url)}{Path.GetExtension(url)}";
            if (System.IO.File.Exists(local_path))
                return File(System.IO.File.ReadAllBytes(local_path), contentType);
            var cookieContainer = new CookieContainer();
            using var handler = new HttpClientHandler() { CookieContainer = cookieContainer };
            using var client = new HttpClient(handler);
            cookieContainer.Add(new Cookie("BX_USER_ID", "c47a549e60a356564f7ae3aff2f365e9", "/", "www.oat.ru"));
            var steam = await client.GetStreamAsync(url);
            SaveImage(local_path, url);
            return File(ReadFully(steam), contentType);
        }

        private void SaveImage(string filePath, string url) => new Task(async () =>
        {
            using var client = new HttpClient();
            try
            {
                var steam = await client.GetStreamAsync(url);
                using FileStream outputFileStream = new FileStream(filePath, FileMode.Create);
                await steam.CopyToAsync(outputFileStream);
            }
            catch 
            { 
                System.IO.File.Delete(filePath);
            }
        }).Start();

        private static byte[] ReadFully(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                    ms.Write(buffer, 0, read);
                
                return ms.ToArray();
            }
        }
    }
}
