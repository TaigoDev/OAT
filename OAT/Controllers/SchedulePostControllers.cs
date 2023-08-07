using Microsoft.AspNetCore.Mvc;
using OAT.Readers;

namespace OAT.Controllers
{
    [AuthorizeRoles(Enums.Role.admin, Enums.Role.schedule_manager)]
    public class SchedulePostControllers : Controller
    {

        [HttpPost("api/schedule/{building}/upload")]
        public async Task<IActionResult> UploadSchedule(string building, IFormFile file)
        {
            var filename = ScheduleReader.GetFilenameByBuilding(building);
            if (file is null || file.Length == 0 || filename is null || Path.GetExtension(file.FileName) is not ".xml")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!await AuthorizationController.CheckLogin(User.Username(), User.Password()))
                return StatusCode(StatusCodes.Status401Unauthorized);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{filename}.xml");
            Utils.FileDelete(path);

            using Stream fileStream = new FileStream(path, FileMode.Create);
            file.CopyTo(fileStream);

            Logger.Info($"Пользователь {User.Username()} обновил расписание для {building}\nIP: {HttpContext.UserIP()}");
            ScheduleReader.init();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
