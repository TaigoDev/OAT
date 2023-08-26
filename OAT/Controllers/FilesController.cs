using Microsoft.AspNetCore.Mvc;
using OAT.Utilities;

namespace OAT.Controllers
{
    public class FilesController : Controller
    {

        [HttpPost("api/schedule/changes/{building}/upload"), AuthorizeRoles(Enums.Role.www_manager_changes_ALL, Enums.Role.www_admin), NoCache]
        public async Task<IActionResult> UploadChangesSchedule(string building, IFormFile file)
        {
            if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xls")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!await AuthorizationController.ValidateCredentials(User, HttpContext.UserIP()))
                return StatusCode(StatusCodes.Status401Unauthorized);

            if (!Permissions.HavePermissionСampusById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{building}-changes.xls");
            Utils.FileDelete(path);
            using Stream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Dispose();
            Logger.Info($"Пользователь {User.GetUsername()} обновил файл с изменением расписания для {building}\nIP: {HttpContext.UserIP()}");
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("api/schedule/changes/{building}/download"), NoCache]
        public IActionResult DownloadChanges(string building)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{building}-changes.xls");
            if (!System.IO.File.Exists(path))
                return Redirect("/timetable/ClassesChanges");

            return File(System.IO.File.ReadAllBytes(path), "application/xls", $"{building}-changes-{new Random().Next()}.xml");
        }

    }
}
