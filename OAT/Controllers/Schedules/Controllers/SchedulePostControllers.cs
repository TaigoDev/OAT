using Microsoft.AspNetCore.Mvc;
using OAT.Controllers.Schedules.Readers;
using OAT.Controllers.Security;
using OAT.Utilities;
using OAT.Utilities.Telegram;

namespace OAT.Controllers.Schedules.Controllers
{
	[AuthorizeRoles(Enums.Role.www_manager_schedule_ALL, Enums.Role.www_admin)]
	public class SchedulePostControllers : Controller
	{

		[HttpPost("api/schedule/{building}/upload"), NoCache]
		public async Task<IActionResult> UploadSchedule(string building, IFormFile file)
		{
			if (file is null || file.Length == 0 || Path.GetExtension(file.FileName) is not ".xml")
				return StatusCode(StatusCodes.Status400BadRequest);

			if (!Permissions.RightsToBuildingById(User.GetUsername(), building))
				return StatusCode(StatusCodes.Status406NotAcceptable);

			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building}.xml");
			var latest = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", "latest", $"{building}.xml");

			FileUtils.FileDelete(latest);
			System.IO.File.Move(path, latest);

			using Stream fileStream = new FileStream(path, FileMode.Create);
			await file.CopyToAsync(fileStream);
			fileStream.Dispose();
			Logger.Info($"Пользователь {User.GetUsername()} обновил расписание для {building}\nIP: {HttpContext.UserIP()}");
			Runs.InThread(async () => await TimeTableBot.onChangeMainSchedule(building, path));
			await ScheduleReader.init();
			return StatusCode(StatusCodes.Status200OK);
		}

	}
}
