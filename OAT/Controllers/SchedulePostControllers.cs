using Microsoft.AspNetCore.Mvc;
using OAT.Readers;
using OAT.Utilities;

namespace OAT.Controllers
{
    [AuthorizeRoles(Enums.Role.www_manager_schedule_ALL, Enums.Role.www_admin)]
    public class SchedulePostControllers : Controller
    {

        [HttpPost("api/schedule/{building}/upload"), NoCache]
        public async Task<IActionResult> UploadSchedule(string building, IFormFile file)
        {
            var filename = ScheduleUtils.GetFilenameByBuilding(building);
            if (file is null || file.Length == 0 || filename is null || Path.GetExtension(file.FileName) is not ".xml")
                return StatusCode(StatusCodes.Status400BadRequest);

            if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
                return StatusCode(StatusCodes.Status406NotAcceptable);

            var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{filename}.xml");
            Utils.FileDelete(path);

            using Stream fileStream = new FileStream(path, FileMode.Create);
            await file.CopyToAsync(fileStream);
            fileStream.Dispose();
            Logger.Info($"Пользователь {User.GetUsername()} обновил расписание для {building}\nIP: {HttpContext.UserIP()}");
            await ScheduleReader.init();
            return StatusCode(StatusCodes.Status200OK);
        }

    }
}
