using Microsoft.AspNetCore.Components.Forms;
using OAT.Controllers.Schedules.Readers;
using OAT.Entities.Enums;
using OAT.Utilities;
using OAT.Utilities.Telegram;

namespace OMAVIAT.Services.AdminPanel
{
	public class FilesHelper
	{
		public static async Task<string?> ChangesTestFile(Building? building, IBrowserFile file)
		{
			var filename = $"{Path.GetRandomFileName()}-changes.xlsx";
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", filename);
			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
			var response = await TimeTableBot.TestChangesInSchedule(building.ConvertToString(), path, filename);
			File.Delete(path);
			return response;
		}

		public static async Task<string?> PracticeTestFile(Building? building, IBrowserFile file)
		{
			var filename = $"{Path.GetRandomFileName()}-changes.xlsx";
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", filename);
			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
			var response = await TimeTableBot.TestPractice(building.ConvertToString(), path, filename);
			File.Delete(path);
			return response;
		}

		public static async Task ChangesSaveFile(Building? building, IBrowserFile file)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building.ConvertToString()}-changes.xlsx");
			var latest = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", "latest", $"{building.ConvertToString()}-changes.xlsx");
			FileUtils.FileDelete(latest);
			File.Move(path, latest);

			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
			Runs.InThread(async () => await TimeTableBot.onChangesInSchedule(building.ConvertToString(), path));
		}

		public static async Task ScheduleSaveFile(Building? building, IBrowserFile file)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{building.ConvertToString()}.xml");
			var latest = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", "latest", $"{building.ConvertToString()}.xml");

			FileUtils.FileDelete(latest);
			File.Move(path, latest);

			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
			Runs.InThread(async () => await TimeTableBot.onChangeMainSchedule(building.ConvertToString(), path));
			Runs.InThread(async () => await ScheduleReader.init());
		}

		public static async Task SessionSaveFile(string filename, Building? building, IBrowserFile file)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building.ConvertToString(), $"{StringUtils.ConvertStringToHex(filename)}.xlsx");
			FileUtils.FileDelete(path);

			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
		}

		public static async Task PracticeSaveFile(string filename, Building? building, IBrowserFile file)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building.ConvertToString(), $"{StringUtils.ConvertStringToHex(filename)}.xlsx");
			FileUtils.FileDelete(path);

			await using FileStream fs = new(path, FileMode.Create);
			await file.OpenReadStream(maxAllowedSize: 1024 * 1024 * 100).CopyToAsync(fs);
			await fs.DisposeAsync();
		}
	}
}
