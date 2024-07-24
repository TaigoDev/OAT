using OAT.Controllers.Schedules;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using System.Xml;

namespace OMAVIAT.Services.Schedule.MainSchedule
{
	public class ScheduleReader
	{

		public static List<CorpusSchedule> schedules = [];

		public static async Task ReadAllAsync()
		{
			schedules.Clear();
			for (int i = 1; i <= 4; i++)
				await ReadAsync(i);
		}

		private static async Task ReadAsync(int Id)
		{
			var file = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{Id}.xml");
			if (!File.Exists(file))
			{
				Logger.Warning($"[ScheduleReader]: Не удалось найти файл {file}");
				return;
			}

			var content = await ScheduleUtils.ReadFileAsync(file);
			if (content is null) return;

			var xDoc = new XmlDocument();
			xDoc.LoadXml(content);

			var timetable = xDoc.GetElementsByTagName("timetable").Item(0);
			if (timetable is null)
			{
				Logger.Warning("[ScheduleReader]: Не удалось получить элемент timetable в XML файле");
				return;
			}

			var schedule = schedules.FirstOrDefault(e => e.building == Id);
			if (schedule is not null)
				schedules.Remove(schedule);
			schedule = new CorpusSchedule()
			{
				building = Id,
			};

			var rings = xDoc.GetElementsByTagName("rings").Item(0);
			if (rings is not null)
			{
				foreach (XmlNode ring in rings)
				{
					var start = ring.GetAttributeValue("begin_time");
					var end = ring.GetAttributeValue("end_time");
					var number = ring.GetAttributeValue("number");
					if (end is null || start is null || !int.TryParse(number, null, out var bell_id))
						continue;

					schedule.bells.Add(new ScheduleBell
					{
						start = start,
						end = end,
						Id = bell_id
					});
				}
			}
			else
				Logger.Error("[ScheduleReader]: Размытие пустой ссылки у lessons_time");

			schedules.Add(schedule);

			foreach (XmlNode group in timetable)
			{
				try
				{
					var name = group.GetAttributeValue("name");
					if (name == null)
					{
						Logger.Warning("[ScheduleReader]: ReadAsync: Не удалось получить имя группы");
						continue;
					}

					var cursChar = name.FirstOrDefault(char.IsDigit);
					if (cursChar is default(char) || !int.TryParse(cursChar.ToString(), out var curs))
					{
						Logger.Warning("[ScheduleReader]: ReadAsync: Не удалось получить имя группы");
						continue;
					}

					schedule.groups.Add(new Entities.Schedule.Schedule
					{
						name = name,
						Curs = curs,
						type = ScheduleType.Group,
						Weeks = GetWeeks(group, name, Id)
					});

				}
				catch (Exception ex)
				{
					var name = group.GetAttributeValue("name");
					Logger.Error($"Ошибка загрузки группы {name} из b{Id}. \nОшибка: {ex}");
				}
			}
		}

		private static List<ScheduleWeek> GetWeeks(XmlNode xml_group, string groupName, int corpus)
		{

			return xml_group.GetChilds().Select((week, index) =>
			{
				var attribute = week.GetAttributeValue("number");
				if (attribute is null || !int.TryParse(attribute, null, out var Id))
					throw new Exception("[ScheduleReader]: Не удалось получить Id недели");

				var days = GetDays(week, index, groupName, corpus);
				return new ScheduleWeek()
				{
					Days = days,
					Type = Id == 1 ? ScheduleWeekType.First : ScheduleWeekType.Second,
				};
			}).ToList();
		}

		private static List<ScheduleDay> GetDays(XmlNode week, int week_id, string groupName, int corpus)
		{
			return week.GetChilds().Select((day, index) =>
			{
				var attribute = week.GetAttributeValue("number");
				if (attribute is null || !int.TryParse(attribute, null, out var Id))
					throw new Exception("[ScheduleReader]: Не удалось получить имя дня");

				var lessons = GetLessons(day, week_id, index, groupName, corpus);
				return new ScheduleDay()
				{
					Type = Id,
					lessons = lessons,
				};
			}).ToList();

		}

		private static List<ScheduleLesson> GetLessons(XmlNode day, int week_id, int day_id, string groupName, int corpus)
		{
			return day.GetChilds().ConvertAll(lesson =>
			{
				var attribute = lesson.GetAttributeValue("number");
				if (attribute is null || !int.TryParse(attribute, null, out var Id))
					throw new Exception("[ScheduleReader]: Не удалось получить Id урока");


				return SetSchedule(lesson, week_id, day_id, groupName, Id, corpus);
			}).SelectMany(i => i).Distinct().ToList();
		}

		private static List<ScheduleLesson> SetSchedule(XmlNode lesson, int week_id, int day_id, string groupName, int lesson_id, int corpus)
		{
			return lesson.GetChilds().ConvertAll(subgroup =>
			{
				var attributes = subgroup.Attributes!;
				var number = attributes.GetAttributeValue("number");
				var subject = attributes.GetAttributeValue("short_subject");
				var teacherFullName = attributes.GetAttributeValue("teacher");
				var cabinet = attributes.GetAttributeValue("cabinet");

				if (number is null || !int.TryParse(number, null, out var Id))
					throw new Exception("[ScheduleReader]: Не удалось получить Id подгруппы");

				var schedule = schedules.FirstOrDefault(e => e.building == corpus);
				if (schedule is null)
					throw new Exception("[ScheduleReader]: Не удалось получить расписание корпуса");

				cabinet ??= "Отсутсвует";
				if (string.IsNullOrWhiteSpace(subject) || string.IsNullOrWhiteSpace(teacherFullName))
					throw new Exception("[ScheduleReader]: Один из элементов для состовления расписания является пустым");

				SetTeacherSchedule(Id, schedule, subject, teacherFullName, cabinet, week_id, day_id, groupName, lesson_id);
				SetCabinetSchedule(Id, schedule, subject, teacherFullName, cabinet, week_id, day_id, groupName, lesson_id);
				return new ScheduleLesson 
				{ 
					Cabinet = cabinet,
					Group = groupName,
					Name = subject,
					Teacher = teacherFullName,
					Id = lesson_id,
					subGroupId = Id,
				};
			});
		}

		private static void SetCabinetSchedule(int Id, CorpusSchedule schedule, string subject, string teacherFullName, string cabinet_number, int week_id, int day_id, string groupName, int lesson_id)
		{
			var cabinet = schedule.cabinets.FirstOrDefault(e => e.name == cabinet_number);
			if (cabinet is null)
			{
				cabinet = new Entities.Schedule.Schedule
				{
					name = cabinet_number,
					type = ScheduleType.Cabinet,
					Weeks = [],
				};
				schedule.cabinets.Add(cabinet);
			}

			var week = cabinet.Weeks.FirstOrDefault(e => e.Type == (week_id == 1 ? ScheduleWeekType.First : ScheduleWeekType.Second));
			if (week is null)
			{
				week = new ScheduleWeek
				{
					Days = [],
					Type = week_id == 1 ? ScheduleWeekType.First : ScheduleWeekType.Second,
				};
				cabinet.Weeks.Add(week);
			}


			var day = week.Days.FirstOrDefault(e => e.Type == day_id);
			if (day is null)
			{
				day = new ScheduleDay
				{
					Type = day_id,
					lessons = [],
				};
				week.Days.Add(day);
			}

			var lesson = new ScheduleLesson
			{
				Cabinet = cabinet_number,
				Group = groupName,
				Name = subject,
				Teacher = teacherFullName,
				Id = lesson_id,
				subGroupId = Id,
			};

			day.lessons.Add(lesson);
		}

		private static void SetTeacherSchedule(int Id, CorpusSchedule schedule, string subject, string teacherFullName, string cabinet, int week_id, int day_id, string groupName, int lesson_id)
		{
			var teacher = schedule.teachers.FirstOrDefault(e => e.name.ToSearchView() == teacherFullName.ToSearchView());
			if (teacher is null)
			{
				teacher = new Entities.Schedule.Schedule
				{
					name = teacherFullName,
					type = ScheduleType.Teacher,
					Weeks = [],
				};
				schedule.teachers.Add(teacher);
			}

			var week = teacher.Weeks.FirstOrDefault(e => e.Type == (week_id == 1 ? ScheduleWeekType.First : ScheduleWeekType.Second));
			if (week is null)
			{
				week = new ScheduleWeek
				{
					Days = [],
					Type = week_id == 1 ? ScheduleWeekType.First : ScheduleWeekType.Second,
				};
				teacher.Weeks.Add(week);
			}

			var day = week.Days.FirstOrDefault(e => e.Type == day_id);
			if (day is null)
			{
				day = new ScheduleDay
				{
					Type = day_id,
					lessons = [],
				};
				week.Days.Add(day);
			}

			var lesson = new ScheduleLesson
			{
				Cabinet = cabinet,
				Group = groupName,
				Name = subject,
				Teacher = teacherFullName,
				Id = lesson_id,
				subGroupId = Id,
			};

			day.lessons.Add(lesson);
		}


	}
}
