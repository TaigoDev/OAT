using OAT.Controllers.Schedules.Readers;

namespace OAT.Utilities.Telegram
{
	public class TimeTableBot
	{


		public static async Task onChangesInSchedule(string building, string xlsx, bool IsNext = false)
		{
			try
			{
				await ChangesController.UpdateCorpusChanges(int.Parse(string.Join("", building.FirstOrDefault(char.IsDigit))));

				var client = new HttpClient();
				client.Timeout = TimeSpan.FromMinutes(2);
				using var request = new HttpRequestMessage(HttpMethod.Post,
					$"{Configurator.timetable.url}/api/alerts/changes/schedule/{building}");
				using var content = new MultipartFormDataContent
				{
					{ new StringContent(Configurator.timetable.token), "token" },
				};

				request.Content = content;
				await client.SendAsync(request);
			}
			catch (Exception ex)
			{
				var time = IsNext ? 30 : 5;
				Logger.Error($"Ошибка при отправке запроса к боту расписания. Попробую ещё раз через {time} минут...\n{ex}");
				await Task.Delay(new TimeSpan(0, time, 0));
				Runs.InThread(async () => await onChangesInSchedule(building, xlsx, true));
			}
		}

		public static async Task<string?> TestChangesInSchedule(string building, string xlsx, string filename)
		{
			try
			{
				var client = new HttpClient();
				client.Timeout = TimeSpan.FromMinutes(2);
				await using var stream = File.OpenRead(xlsx);
				using var request = new HttpRequestMessage(HttpMethod.Post,
					$"{Configurator.timetable.url}/api/test/changes/{building}");
				using var content = new MultipartFormDataContent
				{
					{ new StringContent(Configurator.timetable.token), "token" },
					{ new StringContent(filename), "filename" },
				};

				request.Content = content;
				var response = await client.SendAsync(request);
				if (response.StatusCode is System.Net.HttpStatusCode.OK)
					return null;

				if (response.StatusCode is System.Net.HttpStatusCode.BadRequest)
					return await response.Content.ReadAsStringAsync();

				return "❌ Произошла ошибка тестирования расписания. Обратитесь в службу информатизации вопрос к разработчикам сайта!";
			}
			catch (Exception ex)
			{
				Logger.Error($"Ошибка при отправке запроса к боту расписания.\n{ex}");
			}
			return null;

		}

		public static async Task onChangeMainSchedule(string building, string xml)
		{

			try
			{
				var client = new HttpClient();
				await using var stream = File.OpenRead(xml);
				using var request = new HttpRequestMessage(HttpMethod.Post,
					$"{Configurator.timetable.url}/api/alerts/schedule/{building}");
				using var content = new MultipartFormDataContent
				{
					{ new StringContent(Configurator.timetable.token), "token" },
					{ new StreamContent(stream), "file", $"{building}.xml" }
				};

				request.Content = content;
				await client.SendAsync(request);
			}
			catch (Exception ex)
			{
				Logger.Error($"Ошибка при отправке запроса к боту расписания. Попробую ещё раз через 25 минут...\n{ex}");
			}
		}
	}
}
