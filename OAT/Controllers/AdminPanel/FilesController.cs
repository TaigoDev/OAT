using Microsoft.AspNetCore.Mvc;
using MimeTypes;
using OAT.Controllers.Security;
using OAT.Utilities;
using OAT.Utilities.Telegram;

namespace OAT.Controllers.AdminPanel
{
	public class FilesController : Controller
	{
		#region Schedule changes 
		[HttpGet("changes/{building}/09876635187285765187736186318263782/download"), NoCache]
		public async Task<IActionResult> DownloadChanges(string building)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building}-changes.xlsx");
			if (!System.IO.File.Exists(path))
				return Redirect("/timetable/ClassesChanges");

			return File(await System.IO.File.ReadAllBytesAsync(path), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{building}Changes.xlsx");
		}

		#endregion
		#region Sessions

	
		[HttpGet("api/sessions/{building}/files"), NoCache]
		public IActionResult GetSessionsFiles(string building)
		{
			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building);
			var files = Directory.GetFiles(folder, "*.xlsx", SearchOption.TopDirectoryOnly).ToList();
			var names = files.ConvertAll(e => StringUtils.ConvertHexToString(Path.GetFileName(e).Replace(".xlsx", "")));

			return Ok(names.toJson());
		}

		[HttpGet("api/sessions/{building}/{filename}/download")]
		public async Task<IActionResult> DownloadSessionFile(string building, string filename)
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{filename}.xlsx");
			if (!System.IO.File.Exists(file))
				return StatusCode(StatusCodes.Status404NotFound);
			return File(await System.IO.File.ReadAllBytesAsync(file), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{building}Sessions.xlsx");
		}

		#endregion
		#region Practice 
		[HttpGet("api/practice/{building}/files"), NoCache]
		public IActionResult GetPracticeFiles(string building)
		{
			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building);
			var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToList();
			var names = files.ConvertAll(e => StringUtils.ConvertHexToString(Path.GetFileName(e).Replace(Path.GetExtension(e), "")));

			return Ok(names.toJson());
		}


		[HttpGet("api/practice/{building}/{filename}/download"), NoCache]
		public async Task<IActionResult> DownloadPracticeFile(string building, string filename)
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building, StringUtils.ConvertHexToString(filename));
			if (!System.IO.File.Exists(file))
				return StatusCode(StatusCodes.Status404NotFound);
			return File(await System.IO.File.ReadAllBytesAsync(file), MimeTypeMap.GetMimeType(Path.GetExtension(file)), Path.GetFileName(StringUtils.ConvertHexToString(filename)));
		}


		#endregion
	}
}
