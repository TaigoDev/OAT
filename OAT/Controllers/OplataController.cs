
using Microsoft.AspNetCore.Mvc;
using Net.Codecrete.QrCodeGenerator;
using OAT.Readers;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Svg;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Text;

namespace OAT.Controllers
{
    public class OplataController : Controller
    {


		[HttpGet("pay/download/{filename}")]
		public IActionResult DownloadFile(string filename)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "pay", filename);
			var bytes = System.IO.File.ReadAllBytes(path);
			System.IO.File.Delete(path);
			return File(bytes, "image/svg+xml");
		}

		[HttpGet("pay/contract/search")]
		public IActionResult Search([FromQuery] string documentId, [FromQuery] string documentDate,
			[FromQuery] string studentFullName, [FromQuery] string group, [FromQuery] string FullName) =>
			Ok(ContractReader.IsContract(e => e.NomKontrakt == documentId && e.DataKontrakt == documentDate &&
			e.FullName == studentFullName && e.Gruppa == group && e.Zakazchik == FullName));


	}
}
