using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;

namespace OAT.Controllers
{
    public class FilesController : Controller
    {

        [HttpPost("api/schedule/changes/{building}/upload"), AuthorizeRoles(Enums.Role.schedule_manager, Enums.Role.admin)]
        public async Task<IActionResult> UploadChangesSchedule(string building, IFormFile file)
        {
            if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xls")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                return StatusCode(StatusCodes.Status401Unauthorized);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{building}-changes.xls");
            Utils.FileDelete(path);
            using Stream fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);
            Logger.Info($"Пользователь {User.Username()} обновил файл с изменением расписания для {building}\nIP: {HttpContext.UserIP()}");
            return StatusCode(StatusCodes.Status200OK);
        }

        [HttpGet("api/schedule/changes/{building}/download")] 
        public IActionResult DownloadChanges(string building)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{building}-changes.xls");
            if (!System.IO.File.Exists(path))
                return Redirect("/timetable/ClassesChanges");

            return File(System.IO.File.ReadAllBytes(path), "application/xls", $"{building}-changes-{new Random().Next()}.xml");
        }

    }
}
