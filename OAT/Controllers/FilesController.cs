using Microsoft.AspNetCore.Mvc;
using OAT.Utilities;
using System.IO;
using System.Net.WebSockets;

namespace OAT.Controllers
{
    public class FilesController : Controller
    {

        [HttpPost("api/schedule/changes/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_changes_ALL, Enums.Role.www_admin), NoCache]
        public async Task<IActionResult> UploadChangesSchedule(string building, IFormFile file)
        {
            if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xls")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!Permissions.HavePermissionСampusById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building}-changes.xls");
            Utils.FileDelete(path);

            using Stream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Dispose();

            Logger.Info($"Пользователь {User.GetUsername()} обновил файл с изменением расписания для {building}\nIP: {HttpContext.UserIP()}");
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpPost("api/sessions/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_files_sessions_ALL, Enums.Role.www_admin), NoCache]
        public async Task<IActionResult> UploadSessionsFile(string building, string filename, IFormFile file)
        {
            if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xls")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!Permissions.HavePermissionСampusById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{Utils.ConvertStringToHex(filename)}.xls");
            Utils.FileDelete(path);

            using Stream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Dispose();

            Logger.Info($"Пользователь {User.GetUsername()} добавил файл сессии для {building}\nIP: {HttpContext.UserIP()}");
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("api/sessions/{building}/files"), NoCache]
        public async Task<IActionResult> GetSessionsFiles(string building)
        {
            if (!Permissions.HavePermissionСampusById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building);
            var files = Directory.GetFiles(folder, "*.xls", SearchOption.TopDirectoryOnly).ToList();
            var names = files.ConvertAll(e => Utils.ConvertHexToString(Path.GetFileName(e).Replace(".xls", "")));

            return Ok(names.toJson());
        }

        [HttpDelete("api/sessions/{building}/delete"), AuthorizeRoles(Enums.Role.www_manager_files_sessions_ALL, Enums.Role.www_admin), NoCache]
        public async Task<IActionResult> RemoveSessionsFile(string building, string filename)
        {
            if (!Permissions.HavePermissionСampusById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var File = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{Utils.ConvertStringToHex(filename)}.xls");
            Utils.FileDelete(File);
            Logger.Info($"{User.GetUsername()} удалил файл сессии {filename}");
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("api/sessions/{building}/{filename}/download")]
        public async Task<IActionResult> DownloadSessionFile(string building, string filename)
        {
            var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building, $"{filename}.xls");
            if (!System.IO.File.Exists(file))
                return StatusCode(StatusCodes.Status404NotFound);
            return File(await System.IO.File.ReadAllBytesAsync(file), "application/xls", $"{building}-sessions-{new Random().Next()}.xls");
        }

        [HttpGet("api/schedule/changes/{building}/download"), NoCache]
        public IActionResult DownloadChanges(string building)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{building}-changes.xls");
            if (!System.IO.File.Exists(path))
                return Redirect("/timetable/ClassesChanges");

            return File(System.IO.File.ReadAllBytes(path), "application/xls", $"{building}-changes-{new Random().Next()}.xls");
        }




    }
}
