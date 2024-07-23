using OMAVIAT.Entities.Schedule;
using System.Xml;

namespace OMAVIAT.Services.Schedule.MainSchedule
{
	public class ScheduleReader
	{

		public List<CorpusSchedule> schedules = [];

		public async Task ReadAllAsync()
		{
			for (int i = 1; i <= 4; i++)
				await ReadAsync(i);
		}

		public async Task ReadAsync(int Id)
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{Id}.xml");
			if (!File.Exists(file))
			{
				Logger.Warning($"[ScheduleReader]: Не удалось найти файл {file}");
				return;
			}

			var content = await ScheduleUtils.ReadFileAsync(file);
			if(content is null) return;

			var xDoc = new XmlDocument();
			xDoc.LoadXml(content);

			var timetable = xDoc.GetElementsByTagName("timetable").Item(0);
			if(timetable is null)
			{
				Logger.Warning("[ScheduleReader]: Не удалось получить элемент timetable в XML файле");
				return;
			}

			var schedule = schedules.FirstOrDefault(e => e.building == Id);
			if(schedule is not null)
				schedules.Remove(schedule);
			schedule = new CorpusSchedule()
			{
				building = Id,
			};
			schedules.Add(schedule);
		 
			foreach (XmlNode group in timetable)
			{
				try
				{
					var name = group.GetAttributeValue("name");
					if(name == null)
					{
						Logger.Warning("[ScheduleReader]: ReadAsync: Не удалось получить имя группы");
						continue;
					}

					var cursChar = name.FirstOrDefault(char.IsDigit);
					if(cursChar is default(char) || !int.TryParse(cursChar.ToString(), out var curs))
					{
						Logger.Warning("[ScheduleReader]: ReadAsync: Не удалось получить имя группы");
						continue;
					}	

				}
				catch (Exception ex)
				{
					var name = group.GetAttributeValue("name");
					Logger.Error($"Ошибка загрузки группы {name} из b{Id}. \nОшибка: {ex}");
				}
			}
		}




	

	}
}
