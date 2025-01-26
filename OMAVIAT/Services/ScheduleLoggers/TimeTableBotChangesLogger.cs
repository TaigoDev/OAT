using NLog;
using OMAVIAT.Schedule.Entities;
using OMAVIAT.Schedule.Entities.Enums;

namespace OMAVIAT.Services.ScheduleLoggers;

public class TimeTableBotChangesLogger : IFileLogger
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public async Task NotifyAboutFileChangesAsync(Corpus corpus)
	{
		try
		{
			var client = new HttpClient();
			client.Timeout = TimeSpan.FromMinutes(2);
			using var request = new HttpRequestMessage(HttpMethod.Post,
				$"{Configurator.Timetable.url}/api/alerts/changes/schedule/{corpus}");
			using var content = new MultipartFormDataContent
			{
				{
					new StringContent(Configurator.Timetable.token), "token"
				}
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