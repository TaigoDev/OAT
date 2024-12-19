using Microsoft.AspNetCore.Mvc;

namespace OMAVIAT.Services.Workers {
	public class WorkersImagesController : Controller {
		[HttpGet("api/images/teachers/{FullName?}")]
		public async Task<IActionResult> GetTeacherPhoto(string? FullName)
		{
			if (FullName is null)
				return Redirect("/images/basic/unnamed.jpg");

			var workedFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "people");
			var files = Directory.GetFiles(workedFolder, $"{FullName}.*");
			if (!files.Any())
				return Redirect("/images/basic/unnamed.jpg");

			return File(await System.IO.File.ReadAllBytesAsync(files.First()), "application/png");
		}

	}
}
