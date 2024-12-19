using OMAVIAT.Schedule.Entities;
using OMAVIAT.Schedule.Entities.Enums;

namespace OMAVIAT.Services.ScheduleLoggers;

public class TimeTableBotChangesLogger : IFileLogger {
	public async Task NotifyAboutFileChangesAsync(Corpus corpus)
	{
		try
		{
			var client = new HttpClient();
			client.Timeout = TimeSpan.FromMinutes(2);
			using var request = new HttpRequestMessage(HttpMethod.Post,
			$"{Configurator.timetable.url}/api/alerts/changes/schedule/{corpus}");
			using var content = new MultipartFormDataContent
			{
				{
					new StringContent(Configurator.timetable.token), "token"
				},
			};

			request.Content = content;
			await client.SendAsync(request);
		}
		catch (Exception ex)
		{
			Logger.Error(ex);
		}

	}
}
