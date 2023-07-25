using System.Security.Cryptography.X509Certificates;
using System.Xml;
using static OAT.Readers.ScheduleReader;

namespace OAT.Readers
{
    public class ScheduleReader
    {

        public static List<Group> groups = new List<Group>();

        public static void init()
        {
            try
            {
                var xDoc = new XmlDocument();
                var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule.xml");
                if (!File.Exists(path))
                {
                    Logger.Error("Файл schedule.xml не найден!");
                    return;
                }
                var content = File.ReadAllText(path);
                xDoc.LoadXml(content.Replace("windows-1251", "utf-8"));
                XmlNode xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
                var total = xml_groups.ChildNodes.Count;
                int current = 0;
                using var progress = new ProgressBar();

                foreach (XmlNode xml_group in xml_groups)
                {
                    groups.Add(new Group(xml_group.Attributes!["name"]!.Value, GetWeeks(xml_group)));
                    current++;
                    progress.Report((double)(100f / total) * current);
                }
                Logger.Info($"Расписание успешно загружено за {progress.stopWatch.ElapsedMilliseconds} ms");
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

        
    }
}
