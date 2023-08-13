using Microsoft.AspNetCore.Mvc;
using OAT.Readers;

namespace OAT.Controllers
{
    public class OplataController : Controller
    {


        [HttpGet("pay/download/{filename}")]
        public IActionResult DownloadFile(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "pay", filename);
            if (!System.IO.File.Exists(path))
                return NotFound();

            var bytes = System.IO.File.ReadAllBytes(path);
            System.IO.File.Delete(path);
            return File(bytes, "image/svg+xml");
        }

        [HttpGet("pay/contract/search")]
        public IActionResult Search([FromQuery] string documentId, [FromQuery] string documentDate,
            [FromQuery] string studentFullName, [FromQuery] string group, [FromQuery] string FullName)
        {

           Logger.Info($"Поиск {documentId} {documentDate} {studentFullName} {group} {FullName}");
           return Ok(ContractReader.IsContract(e => e.NomKontrakt.ToLower() == documentId.ToLower() && e.DataKontrakt.ToLower() == documentDate.ToLower() &&
            e.FullName.ToLower() == studentFullName.ToLower() && e.Gruppa.ToLower() == group.ToLower() && e.Zakazchik.ToLower() == FullName.ToLower()));

        }
    }
}
