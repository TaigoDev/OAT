using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OMAVIAT.Utilities;

namespace OMAVIAT.Controllers.Natives;

public class StaticController : Controller
{
	[HttpGet("static/documents/{hex}/download")]
	public async Task<IActionResult> Download(string hex)
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources/static/documents",
			StringUtils.ConvertHexToString(hex));
		if (!System.IO.File.Exists(path))
			return NotFound("File not found");
		return File(await System.IO.File.ReadAllBytesAsync(path), ContentType(StringUtils.ConvertHexToString(hex)));
	}

	private string ContentType(string url)
	{
		new FileExtensionContentTypeProvider().TryGetContentType(url, out var contentType);
		return contentType ?? "application/file";
	}
}