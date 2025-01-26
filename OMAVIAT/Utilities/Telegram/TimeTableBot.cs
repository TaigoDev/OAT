using System.Net;
using NLog;

namespace OMAVIAT.Utilities.Telegram;

public class TimeTableBot
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static async Task<string?> TestPractice(string building, string xlsx, string filename)
	{
		try
		{
			var client = new HttpClient();
			client.Timeout = TimeSpan.FromMinutes(2);
			await using var stream = File.OpenRead(xlsx);
			using var request = new HttpRequestMessage(HttpMethod.Post,
				$"{Configurator.Timetable.url}/api/test/practice/schedule/{building}");
			using var content = new MultipartFormDataContent
			{
				{
					new StringContent(Configurator.Timetable.token), "token"
				},
				{
					new StringContent(filename), "filename"
				}
			};

			request.Content = content;
			var response = await client.SendAsync(request);
			if (response.StatusCode is HttpStatusCode.OK)
				return null;

			if (response.StatusCode is HttpStatusCode.BadRequest)
				return await response.Content.ReadAsStringAsync();

			return
				"❌ Произошла ошибка тестирования расписания. Обратитесь в службу информатизации вопрос к разработчикам сайта!";
		}
		catch (Exception ex)
		{
			Logger.Error($"Ошибка при отправке запроса к боту расписания.\n{ex}");
		}

		return null;
	}
}