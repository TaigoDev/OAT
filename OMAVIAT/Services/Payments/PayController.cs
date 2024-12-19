using Microsoft.AspNetCore.Mvc;

namespace OMAVIAT.Services.Payments {
	public class PayController : Controller {
		[HttpGet("pay/download/{filename}"), NoCache]
		public IActionResult DownloadFile(string filename)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay", filename);
			if (!System.IO.File.Exists(path))
				return NotFound();

			var bytes = System.IO.File.ReadAllBytes(path);
			System.IO.File.Delete(path);
			return File(bytes, "image/svg+xml");
		}

		[HttpGet("pay/contract/search"), NoCache]
		public IActionResult Search([FromQuery] string documentId, [FromQuery] string documentDate,
			[FromQuery] string studentFullName, [FromQuery] string group, [FromQuery] string FullName) =>
			Ok(ContractReader.IsContract(e =>
				e.documentId.ToSearchView() == documentId.ToSearchView() &&
				e.documentDate.ToSearchView() == documentDate.ToSearchView() &&
				e.studentFullName.ToSearchView() == studentFullName.ToSearchView() &&
				e.Group.ToSearchView() == group.ToSearchView() &&
				e.FullName.ToSearchView() == FullName.ToSearchView()));

	}
}
