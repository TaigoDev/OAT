#pragma warning disable CS8602
using System.Xml;

namespace OAT.Readers
{
    public class ScheduleReader
    {

        public static List<Group> b1 = new List<Group>();
        public static List<Group> b2 = new List<Group>();
        public static List<Group> b3 = new List<Group>();
        public static List<Group> b4 = new List<Group>();

        public static async void init()
        {
            try
            {

                using var progress = new ProgressBar();
                for (int i = 1; i <= 4; i++)
                {
                    var xDoc = new XmlDocument();
                    var xml = LoadXml($"b{i}");

                    if (xml == null)
                        return;

                    xDoc.LoadXml(xml);
                    XmlNode xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
                    var total = xml_groups.ChildNodes.Count;
                    int current = 0;

                    foreach (XmlNode xml_group in xml_groups)
                    {
                        GetBuilding(i).Add(new Group(xml_group.Attributes!["name"]!.Value, GetWeeks(xml_group)));
                        current++;
                        progress.Report((double)((25f / total) * current + (25 * (i - 1))) / 100);
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

        protected static string? LoadXml(string filename)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule", $"{filename}.xml");
            if (!File.Exists(path))
            {
                Logger.Error($"Файл schedule/{filename}.xml не найден!");
                return null;
            }
            var content = File.ReadAllText(path);
            return content.Replace("windows-1251", "utf-8");
        }

        protected static List<Group> GetBuilding(int i)
        {
            switch(i)
            {
                case 1: return b1;
                case 2: return b2;
                case 3: return b3;
                case 4: return b4;
            }
            return new List<Group>();
        }

    }
}
