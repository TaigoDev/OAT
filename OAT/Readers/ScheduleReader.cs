#pragma warning disable CS8602
using System.Text;
using System.Xml;

namespace OAT.Readers
{
    public class ScheduleReader
    {

        public static List<Group> ul_lenina_24 = new List<Group>();
        public static List<Group> ul_b_khmelnickogo_281a = new List<Group>();
        public static List<Group> pr_kosmicheskij_14a = new List<Group>();
        public static List<Group> ul_volkhovstroya_5 = new List<Group>();

        public static async void init()
        {
            try
            {

                using var progress = new ProgressBar();
                for (int i = 1; i <= 4; i++)
                {
                    var xDoc = new XmlDocument();
                    var xml = await LoadXml($"b{i}");

                    if (xml is not null)
                    {
                        xDoc.LoadXml(xml);
                        XmlNode xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
                        var total = xml_groups.ChildNodes.Count;
                        int current = 0;

                        foreach (XmlNode xml_group in xml_groups)
                        {
                            var name = xml_group.Attributes!["name"]!.Value;
                            GetBuilding(i).Add(new Group(
                                name,
                                int.Parse(name.First(e => char.IsDigit(e)).ToString()),
                                GetWeeks(xml_group),
                                GetLessonsTime(xDoc)));
                            current++;
                            progress.Report((double)((25f / total) * current + (25 * (i - 1))) / 100);
                        }
                    }
                }
                Console.WriteLine($"Расписания загружены за {progress.stopWatch.ElapsedMilliseconds} ms");
            }
            catch(Exception ex)
            {
                Logger.Error(ex.ToString());
            }
            
        }

        protected static List<Week> GetWeeks(XmlNode xml_group)
        {
            var weeks = new List<Week>();
            foreach (XmlNode week in xml_group)
                weeks.Add(new Week(int.Parse(week.Attributes!["number"]!.Value), GetDays(week)));
            return weeks;
        }

        protected static List<Day> GetDays(XmlNode week)
        {
            var days = new List<Day>();
            foreach (XmlNode day in week)
                days.Add(new Day(day.Attributes!["name"]!.Value, GetLessons(day)));
            return days;
        }

        protected static List<Lesson> GetLessons(XmlNode day)
        {
            var lessons = new List<Lesson>();
            foreach (XmlNode lesson in day)
                lessons.Add(new Lesson(int.Parse(lesson.Attributes!["number"]!.Value), GetSubgroups(lesson)));
            return lessons;
        }

        protected static List<Subgroup> GetSubgroups(XmlNode lesson)
        {
            var subgroups = new List<Subgroup>();
            foreach (XmlNode subgroup in lesson)
                subgroups.Add(new Subgroup(
                    int.Parse(subgroup.Attributes!["number"]!.Value),
                    subgroup.Attributes!["subject"]!.Value,
                    subgroup.Attributes!["short_subject"]!.Value,
                    subgroup.Attributes!["teacher"]!.Value,
                    subgroup.Attributes!["cabinet"]!.Value));
            return subgroups;
        }

        protected static async Task<string>? LoadXml(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{filename}.xml");
            if (!File.Exists(path))
            {
                Logger.Error($"Файл schedule/{filename}.xml не найден!");
                return null;
            }
            var content = await ReadFile(path);
            return content.Replace("windows-1251", "utf-8");
        }

        protected static List<Group> GetBuilding(int i)
        {
            switch(i)
            {
                case 1: return ul_lenina_24;
                case 2: return ul_b_khmelnickogo_281a;
                case 3: return pr_kosmicheskij_14a;
                case 4: return ul_volkhovstroya_5;
            }
            return new List<Group>();
        }

        public static List<Group>? GetGroupsByBuilding(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            switch (name)
            {
                case "ul_lenina_24": return ul_lenina_24;
                case "ul_b_khmelnickogo_281a": return ul_b_khmelnickogo_281a;
                case "pr_kosmicheskij_14a": return pr_kosmicheskij_14a;
                case "ul_volkhovstroya_5": return ul_volkhovstroya_5;
            }
            return null;
        }

        protected static async Task<string> ReadFile(string path)
        {
            using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            using StreamReader reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251));

            return await reader_encoding.ReadToEndAsync();
        }

        protected static List<string> GetLessonsTime(XmlDocument doc)
        {
            var rings = doc.GetElementsByTagName("rings").Item(0);
            var lessons_time = new List<string>();
            foreach (XmlNode ring in rings)
                lessons_time.Add($"{ring.Attributes["begin_time"]!.Value} - {ring.Attributes["end_time"]!.Value}");
            return lessons_time;
        }

    }
}

