#pragma warning disable CS8602
using OAT.Entities.Schedule;
using System.Diagnostics;
using System.Xml;
using static OAT.Modules.Schedules.ScheduleUtils;

namespace OAT.Modules.Schedules.Readers
{
	public class ScheduleReader
	{

		public static List<Group> ul_lenina_24 = new List<Group>();
		public static List<Group> ul_b_khmelnickogo_281a = new List<Group>();
		public static List<Group> pr_kosmicheskij_14a = new List<Group>();
		public static List<Group> ul_volkhovstroya_5 = new List<Group>();

		public static List<TeacherSchedule> teachers_ul_lenina_24 = new List<TeacherSchedule>();
		public static List<TeacherSchedule> teachers_ul_b_khmelnickogo_281a = new List<TeacherSchedule>();
		public static List<TeacherSchedule> teachers_pr_kosmicheskij_14a = new List<TeacherSchedule>();
		public static List<TeacherSchedule> teachers_ul_volkhovstroya_5 = new List<TeacherSchedule>();
		public static List<string> lesson_times = new List<string>();

		public static async Task init()
		{
			IterateGroupList(e => new List<Group>());
			IterateTeacherList(e => new List<TeacherSchedule>());

			var stopWatch = new Stopwatch();
			stopWatch.Start();
			for (int i = 1; i <= Enums.campus_count; i++)
			{
				try
				{
					var xDoc = new XmlDocument();
					var xml = await LoadXml($"b{i}");

					if (xml is null)
						continue;

					xDoc.LoadXml(xml);
					var xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
					foreach (XmlNode xml_group in xml_groups)
					{
						try
						{
							var name = xml_group.GetAttributeValue("name");
							var curs = name.First(e => char.IsDigit(e)).ToString().ToInt32();

							GetBuilding(i).Add(new Group(name, curs,
								GetWeeks(xml_group, name, GetTeacherScheduleListByBuildingId(i))));
						}
						catch (Exception ex)
						{
							Logger.Error($"Ошибка загрузки группы {xml_group.GetAttributeValue("name")} из b{i}. \nОшибка: {ex}");
						}
					}

				}
				catch (Exception ex)
				{
					Logger.Error($"Ошибка загрузки корпуса b{i}. \nОшибка: {ex}");
				}


			}

			IterateTeacherList(e => e.OrderBy(e => e.FullName.ToCharArray().First()).ToList());
			lesson_times = await GetLessonsTime(1);
			stopWatch.Stop();
			Logger.InfoWithoutTelegram($"Расписания загружены за {stopWatch.ElapsedMilliseconds} ms");
		}

		protected static List<Week> GetWeeks(XmlNode xml_group, string groupName, List<TeacherSchedule> TeacherSchedule)
		{
			return xml_group.GetChilds().Select((week, index) =>
				new Week(int.Parse(week.GetAttributeValue("number")), GetDays(week, index, groupName, TeacherSchedule))).ToList();
		}

		protected static List<Day> GetDays(XmlNode week, int week_id, string groupName, List<TeacherSchedule> TeacherSchedule)
		{
			return week.GetChilds().Select((day, index) =>
				new Day(day.GetAttributeValue("name"), GetLessons(day, week_id, index, groupName, TeacherSchedule))).ToList();
		}

		protected static List<Lesson> GetLessons(XmlNode day, int week_id, int day_id, string groupName, List<TeacherSchedule> TeacherSchedule)
		{
			return day.GetChilds().ConvertAll(lesson =>
			{
				var lesson_id = int.Parse(lesson.GetAttributeValue("number"));
				return new Lesson(lesson_id, GetSubgroups(lesson, week_id, day_id, groupName, lesson_id, TeacherSchedule));
			});
		}

		protected static List<Subgroup> GetSubgroups(XmlNode lesson, int week_id, int day_id, string groupName, int lesson_id, List<TeacherSchedule> TeacherSchedule)
		{
			return lesson.GetChilds().ConvertAll(subgroup =>
			{
				var attributes = subgroup.Attributes!;
				var id = attributes.GetAttributeValue("number").ToInt32();
				var subject = attributes.GetAttributeValue("subject");
				var short_subject = attributes.GetAttributeValue("short_subject");
				var teacherFullName = attributes.GetAttributeValue("teacher");
				var cabinet = attributes.GetAttributeValue("cabinet");

				var teacher = TeacherSchedule.FirstOrDefault(e => e.FullName.ToLower() == teacherFullName.ToLower());
				bool InList = teacher is not null ? true : false;
				teacher = teacher is null ? new TeacherSchedule() : teacher;

				teacher.FullName = teacherFullName;
				teacher.GetTeacherLesson(week_id, day_id).Add(new TeacherLesson(lesson_id, short_subject, groupName, cabinet));

				if (!InList)
					TeacherSchedule.Add(teacher);

				return new Subgroup(id, subject, short_subject, teacherFullName, cabinet);
			});
		}


	}
}