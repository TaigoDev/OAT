using OAT.Entities.Schedule;
using OAT.Modules.Schedules.Readers;
using System.Text;
using System.Xml;

namespace OAT.Modules.Schedules
{
	public static class ScheduleUtils
	{
		public static async Task<string?> LoadXml(string filename)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"{filename}.xml");
			if (!File.Exists(path))
			{
				Logger.Warning($"Файл Resources/schedule/{filename}.xml не найден!");
				return null;
			}
			var content = await ReadFile(path);
			return content.Replace("windows-1251", "utf-8");
		}


		public static async Task<string> ReadFile(string path)
		{
			try
			{
				using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
				using StreamReader reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251)!);
				return await reader_encoding.ReadToEndAsync();
			}
			catch
			{
				await Task.Delay(2000);
				using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
				using StreamReader reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251)!);
				return await reader_encoding.ReadToEndAsync();
			}
		}

		public static async Task<List<string>> GetLessonsTime(int i)
		{
			var xDoc = new XmlDocument();
			var xml = await LoadXml($"b{i}");
			xDoc.LoadXml(xml);
			var rings = xDoc.GetElementsByTagName("rings").Item(0);
			var lessons_time = new List<string>();
			foreach (XmlNode ring in rings)
				lessons_time.Add($"{ring.GetAttributeValue("begin_time")} - {ring.GetAttributeValue("end_time")}");
			return lessons_time;
		}

		public static List<Group> GetBuilding(int i) =>
			i switch
			{
				1 => ScheduleReader.ul_lenina_24,
				2 => ScheduleReader.ul_b_khmelnickogo_281a,
				3 => ScheduleReader.pr_kosmicheskij_14a,
				4 => ScheduleReader.ul_volkhovstroya_5,
				_ => new List<Group>()
			};

		public static List<TeacherSchedule> GetTeacherScheduleListByBuildingId(int i) =>
			i switch
			{
				1 => ScheduleReader.teachers_ul_lenina_24,
				2 => ScheduleReader.teachers_ul_b_khmelnickogo_281a,
				3 => ScheduleReader.teachers_pr_kosmicheskij_14a,
				4 => ScheduleReader.teachers_ul_volkhovstroya_5,
				_ => new List<TeacherSchedule>(),
			};

		public static List<Group>? GetGroupsByBuilding(string? name) =>
			name switch
			{
				"ul_lenina_24" => ScheduleReader.ul_lenina_24,
				"ul_b_khmelnickogo_281a" => ScheduleReader.ul_b_khmelnickogo_281a,
				"pr_kosmicheskij_14a" => ScheduleReader.pr_kosmicheskij_14a,
				"ul_volkhovstroya_5" => ScheduleReader.ul_volkhovstroya_5,
				_ => null
			};

		public static List<TeacherSchedule> GetTeacherScheduleByBuilding(string? name) =>
			name switch
			{
				"ul_lenina_24" => ScheduleReader.teachers_ul_lenina_24,
				"ul_b_khmelnickogo_281a" => ScheduleReader.teachers_ul_b_khmelnickogo_281a,
				"pr_kosmicheskij_14a" => ScheduleReader.teachers_pr_kosmicheskij_14a,
				"ul_volkhovstroya_5" => ScheduleReader.teachers_ul_volkhovstroya_5,
				_ => new List<TeacherSchedule>()
			};

		public static string GetFilenameByBuilding(string name) =>
			name switch
			{
				"ul_lenina_24" => "b1",
				"ul_b_khmelnickogo_281a" => "b2",
				"pr_kosmicheskij_14a" => "b3",
				"ul_volkhovstroya_5" => "b4",
				_ => "error_load"
			};


		public static void IterateTeacherList(Func<List<TeacherSchedule>, List<TeacherSchedule>> func)
		{
			for (int i = 1; i <= Enums.campus_count; i++)
			{
				var list = func.Invoke(GetTeacherScheduleListByBuildingId(i));
				GetTeacherScheduleListByBuildingId(i).Clear();
				GetTeacherScheduleListByBuildingId(i).AddRange(list);
			}
		}

		public static void IterateGroupList(Func<List<Group>, List<Group>> func)
		{
			for (int i = 1; i <= Enums.campus_count; i++)
			{
				var list = func.Invoke(GetBuilding(i));
				GetBuilding(i).Clear();
				GetBuilding(i).AddRange(list);
			}
		}

		public static List<XmlNode> GetChilds(this XmlNode node) =>
			node.ChildNodes.Cast<XmlNode>().ToList();

		public static string GetAttributeValue(this XmlAttributeCollection node, string name)
		{
			var attribute = node[name];
			if (attribute is null)
				return string.Empty;

			var value = attribute.Value;
			if (value is null)
				return string.Empty;

			return value.ToString();
		}

		public static string GetAttributeValue(this XmlNode node, string name) =>
			node.Attributes!.GetAttributeValue(name);

		public static List<TeacherLesson> GetTeacherLesson(this TeacherSchedule schedule, int week_id, int day_id) =>
			schedule.weeks[week_id].days[day_id].lessons;


	}
}
