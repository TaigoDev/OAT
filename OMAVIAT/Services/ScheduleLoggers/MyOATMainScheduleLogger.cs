using OMAVIAT.Schedule.Entities;
using OMAVIAT.Schedule.Entities.Enums;

namespace OMAVIAT.Services.ScheduleLoggers;

public class MyOATMainScheduleLogger : IFileLogger
{
	public async Task NotifyAboutFileChangesAsync(Corpus corpus)
	{
		try
		{
			var client = new HttpClient();
			client.Timeout = TimeSpan.FromMinutes(2);
			using var request = new HttpRequestMessage(HttpMethod.Post,
				$"{Configurator.myoat.url}/api/mainschedule/update");
			using var content = new MultipartFormDataContent
			{
				{ new StringContent(corpus.ToString()), "corpus" },
				{ new StringContent(Configurator.myoat.token), "token" },
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