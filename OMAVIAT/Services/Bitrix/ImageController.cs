using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OMAVIAT.Utilities;

namespace OMAVIAT.Controllers.Bitrix.Controllers;

public class ImageController : Controller
{
	[HttpGet]
	[Route("proxing/images/bitrix")]
	public async Task<IActionResult> getImage([FromQuery] string url)
	{
		try
		{
			url = Configurator.Config.BaseUrl + url;

			var local_path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "bitrix",
				$"{StringUtils.SHA226(url)}{Path.GetExtension(url)}");

			if (System.IO.File.Exists(local_path))
				return File(System.IO.File.ReadAllBytes(local_path), ContentType(url));

			SaveImage(local_path, url);
			var steam = await ImageStream(url);
			return File(StringUtils.ReadFully(steam), ContentType(url));
		}
		catch
		{
			return NotFound("Image not found: ImageContoller");
		}
	}

	private string ContentType(string url)
	{
		var contentType = string.Empty;
		new FileExtensionContentTypeProvider().TryGetContentType(url, out contentType);
		return contentType!;
	}

	private async Task<Stream> ImageStream(string url)
	{
		var cookieContainer = new CookieContainer();
		using var handler = new HttpClientHandler
		{
			CookieContainer = cookieContainer
		};
		using var client = new HttpClient(handler);
		cookieContainer.Add(new Cookie("BX_USER_ID", "c47a549e60a356564f7ae3aff2f365e9", "/", "www.oat.ru"));
		return await client.GetStreamAsync(url);
	}

	private void SaveImage(string filePath, string url)
	{
		new Task(async () =>
		{
			using var client = new HttpClient();
			try
			{
				var steam = await client.GetStreamAsync(url);
				using var outputFileStream = new FileStream(filePath, FileMode.Create);
				await steam.CopyToAsync(outputFileStream);
			}
			catch
			{
				FileUtils.FileDelete(filePath);
			}
		}).Start();
	}
}