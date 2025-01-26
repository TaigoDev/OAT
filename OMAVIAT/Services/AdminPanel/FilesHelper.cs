using Microsoft.AspNetCore.Components.Forms;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Schedule.Entities.Enums;
using OMAVIAT.Schedule.Entities.Models;
using OMAVIAT.Schedule.Testers;
using OMAVIAT.Services.ScheduleLoggers;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;

namespace OMAVIAT.Services.AdminPanel;

public class FilesHelper
{
	public static async Task<TestResponseModel> ChangesSaveFile(Building? building, IBrowserFile file)
	{
		var filename = $"{Path.GetRandomFileName()}-changes.xlsx";
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", filename);
		await using FileStream fs = new(path, FileMode.Create);
		await file.OpenReadStream(1024 * 1024 * 100).CopyToAsync(fs);
		await fs.DisposeAsync();
		if (building is null)
			return new TestResponseModel
			{
				Details = "Произошла ошибка. Обратитесь в службу информатизации",
				IsUploaded = false,
				IsSuccess = false
			};
		return await ScheduleChangesTester.LoadAndTestAsync(path, (Corpus)((int)building - 1), new MyOATChangesLogger(),
			new TimeTableBotChangesLogger());
	}

	public static async Task<string?> PracticeTestFile(Building? building, IBrowserFile file)
	{
		var filename = $"{Path.GetRandomFileName()}-changes.xlsx";
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", filename);
		await using FileStream fs = new(path, FileMode.Create);
		await file.OpenReadStream(1024 * 1024 * 100).CopyToAsync(fs);
		await fs.DisposeAsync();
		var response = await TimeTableBot.TestPractice(building.ConvertToString(), path, filename);
		File.Delete(path);
		return response;
	}

	public static async Task<TestResponseModel> ScheduleSaveFile(Building? building, IBrowserFile file)
	{
		var filename = $"{Path.GetRandomFileName()}-schedule.xml";
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", filename);
		await using FileStream fs = new(path, FileMode.Create);
		await file.OpenReadStream(1024 * 1024 * 100).CopyToAsync(fs);
		await fs.DisposeAsync();
		if (building is null)
			return new TestResponseModel
			{
				Details = "Произошла ошибка. Обратитесь в службу информатизации",
				IsUploaded = false,
				IsSuccess = false
			};
		return await MainScheduleTester.LoadAndTestAsync(path, (Corpus)((int)building - 1),
			new MyOATMainScheduleLogger(), new TimeTableBotMainScheduleLogger());
	}

	public static async Task SessionSaveFile(string filename, Building? building, IBrowserFile file)
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", building.ConvertToString(),
			$"{StringUtils.ConvertStringToHex(filename)}.xlsx");
		FileUtils.FileDelete(path);

		await using FileStream fs = new(path, FileMode.Create);
		await file.OpenReadStream(1024 * 1024 * 100).CopyToAsync(fs);
		await fs.DisposeAsync();
	}

	public static async Task PracticeSaveFile(string filename, Building? building, IBrowserFile file)
	{
		var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", building.ConvertToString(),
			$"{StringUtils.ConvertStringToHex(filename)}.xlsx");
		FileUtils.FileDelete(path);

		await using FileStream fs = new(path, FileMode.Create);
		await file.OpenReadStream(1024 * 1024 * 100).CopyToAsync(fs);
		await fs.DisposeAsync();
	}
}