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
		[HttpPost("api/schedule/changes/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_changes_ALL, Enums.Role.www_admin), NoCache]
		public async Task<IActionResult> UploadChangesSchedule(string building, IFormFile file)
		{
			if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xlsx")
				return StatusCode(StatusCodes.Status400BadRequest);

			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var response = await TestChanges(building, file);
			if (response is not null)
				return BadRequest(response);
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building}-changes.xlsx");
			var latest = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", "latest", $"{building}-changes.xlsx");

			FileUtils.FileDelete(latest);
			System.IO.File.Move(path, latest);

			using Stream fileStream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fileStream);
			fileStream.Dispose();
			Runs.InThread(async () => await TimeTableBot.onChangesInSchedule(building, path));

			Logger.Info($"Пользователь {User.GetUsername()} обновил файл с изменением расписания для {building}\nIP: {HttpContext.UserIP()}");
			return StatusCode(StatusCodes.Status200OK);
		}

		[HttpGet("changes/{building}/09876635187285765187736186318263782/download"), NoCache]
		public async Task<IActionResult> DownloadChanges(string building)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building}-changes.xlsx");
			if (!System.IO.File.Exists(path))
				return Redirect("/timetable/ClassesChanges");

			return File(await System.IO.File.ReadAllBytesAsync(path), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", $"{building}Changes.xlsx");
		}

		private async Task<string?> TestChanges(string building, IFormFile file)
		{
			var filename = $"{Path.GetRandomFileName()}-changes.xlsx";
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", filename);
			using Stream fileStream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fileStream);
			fileStream.Dispose();
			var response = await TimeTableBot.TestChangesInSchedule(building, path, filename);
			System.IO.File.Delete(path);
			return response;
		}
		#endregion
		#region Sessions

		[HttpPost("api/sessions/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_files_sessions_ALL, Enums.Role.www_admin), NoCache]
		public async Task<IActionResult> UploadSessionsFile(string building, string filename, IFormFile file)
		{
			if (string.IsNullOrEmpty(filename))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xlsx")
				return StatusCode(StatusCodes.Status400BadRequest);

			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{StringUtils.ConvertStringToHex(filename)}.xlsx");
			FileUtils.FileDelete(path);

			using Stream fileStream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fileStream);
			fileStream.Dispose();

			Logger.Info($"Пользователь {User.GetUsername()} добавил файл сессии для {building}\nIP: {HttpContext.UserIP()}");
			return StatusCode(StatusCodes.Status200OK);
		}

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

		[HttpDelete("api/sessions/{building}/delete"), AuthorizeRoles(Enums.Role.www_manager_files_sessions_ALL, Enums.Role.www_admin), NoCache]
		public IActionResult RemoveSessionsFile(string building, string filename)
		{
			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var File = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{StringUtils.ConvertStringToHex(filename)}.xlsx");
			FileUtils.FileDelete(File);
			Logger.Info($"{User.GetUsername()} удалил файл сессии {filename}");
			return StatusCode(StatusCodes.Status200OK);
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

		[HttpPost("api/practice/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_files_practice_ALL, Enums.Role.www_admin), NoCache]
		public async Task<IActionResult> UploadPracticeFile(string building, string filename, IFormFile file)
		{
			if (string.IsNullOrEmpty(filename))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			if (!ControllersUtils.IsCorrectFile(file, ".xlsx", ".docx"))
				return StatusCode(StatusCodes.Status400BadRequest);

			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building, $"{StringUtils.ConvertStringToHex(filename)}{Path.GetExtension(file.FileName)}");
			FileUtils.FileDelete(path);

			using Stream fileStream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fileStream);
			fileStream.Dispose();

			Logger.Info($"Пользователь {User.GetUsername()} добавил файл практики для {building}\nIP: {HttpContext.UserIP()}");
			return StatusCode(StatusCodes.Status200OK);
		}

		[HttpGet("api/practice/{building}/files"), NoCache]
		public async Task<IActionResult> GetPracticeFiles(string building)
		{
			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building);
			var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToList();
			var names = files.ConvertAll(e => StringUtils.ConvertHexToString(Path.GetFileName(e).Replace(Path.GetExtension(e), "")));

			return Ok(names.toJson());
		}

		[HttpDelete("api/practice/{building}/delete"), AuthorizeRoles(Enums.Role.www_manager_files_practice_ALL, Enums.Role.www_admin), NoCache]
		public IActionResult RemovePracticeFile(string building, string filename)
		{
			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var workedDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building);
			var file = Directory.GetFiles(workedDirectory, $"{StringUtils.ConvertStringToHex(filename)}.*").FirstOrDefault();

			FileUtils.FileDelete(file);
			Logger.Info($"{User.GetUsername()} удалил файл практики {filename}");
			return StatusCode(StatusCodes.Status200OK);
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
