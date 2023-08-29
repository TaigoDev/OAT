using OAT.Readers;
using System.Text;
using System.Xml;

namespace OAT.Utilities
{
    public class ScheduleUtils
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

        public static List<string> GetLessonsTime(XmlDocument doc)
        {
            var rings = doc.GetElementsByTagName("rings").Item(0);
            var lessons_time = new List<string>();
            foreach (XmlNode ring in rings)
                lessons_time.Add($"{ring.Attributes["begin_time"]!.Value} - {ring.Attributes["end_time"]!.Value}");
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
    }
}
