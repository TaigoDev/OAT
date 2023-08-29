#pragma warning disable CS8602
using System.Diagnostics;
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

        public static List<TeacherSchedule> teachers_ul_lenina_24 = new List<TeacherSchedule>();
        public static List<TeacherSchedule> teachers_ul_b_khmelnickogo_281a = new List<TeacherSchedule>();
        public static List<TeacherSchedule> teachers_pr_kosmicheskij_14a = new List<TeacherSchedule>();
        public static List<TeacherSchedule> teachers_ul_volkhovstroya_5 = new List<TeacherSchedule>();
        public static List<string> lesson_times = new List<string>();
        public static async Task init()
        {
            ul_lenina_24.Clear();
            ul_b_khmelnickogo_281a.Clear();
            pr_kosmicheskij_14a.Clear();
            ul_volkhovstroya_5.Clear();
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 1; i <= 4; i++)
            {
                var xDoc = new XmlDocument();
                var xml = await LoadXml($"b{i}");

                if (xml is not null)
                {
                    xDoc.LoadXml(xml);
                    XmlNode xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
                    try
                    {
                        foreach (XmlNode xml_group in xml_groups)
                        {
                            try
                            {
                                var name = xml_group.Attributes!["name"]!.Value;
                                GetBuilding(i).Add(new Group(
                                    name,
                                    int.Parse(name.First(e => char.IsDigit(e)).ToString()),
                                    GetWeeks(xml_group, name, GetTeacherScheduleListByBuildingId(i))));
                            }
                            catch (Exception ex)
                            {
                                Logger.ErrorWithCatch($"Ошибка загрузки группы {xml_group.Attributes!["name"]!.Value} из b{i}. \nОшибка: {ex}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.ErrorWithCatch($"Ошибка загрузки корпуса b{i}. \nОшибка: {ex}");
                    }
                }
                lesson_times = GetLessonsTime(xDoc);

            }
            teachers_ul_lenina_24 = teachers_ul_lenina_24.OrderBy(e => e.FullName.ToCharArray()[0]).ToList();
            teachers_ul_b_khmelnickogo_281a = teachers_ul_b_khmelnickogo_281a.OrderBy(e => e.FullName.ToCharArray()[0]).ToList();
            teachers_pr_kosmicheskij_14a = teachers_pr_kosmicheskij_14a.OrderBy(e => e.FullName.ToCharArray()[0]).ToList();
            teachers_ul_volkhovstroya_5 = teachers_ul_volkhovstroya_5.OrderBy(e => e.FullName.ToCharArray()[0]).ToList();
            stopWatch.Stop();
            Console.WriteLine($"Расписания загружены за {stopWatch.ElapsedMilliseconds} ms");
        }

        protected static List<Week> GetWeeks(XmlNode xml_group, string groupName, List<TeacherSchedule> TeacherSchedule)
        {
            var weeks = new List<Week>();
            foreach (XmlNode week in xml_group)
                weeks.Add(new Week(int.Parse(week.Attributes!["number"]!.Value), GetDays(week, weeks.Count, groupName, TeacherSchedule)));
            return weeks;
        }

        protected static List<Day> GetDays(XmlNode week, int week_id, string groupName, List<TeacherSchedule> TeacherSchedule)
        {
            var days = new List<Day>();
            foreach (XmlNode day in week)
                days.Add(new Day(day.Attributes!["name"]!.Value, GetLessons(day, week_id, days.Count, groupName, TeacherSchedule)));
            return days;
        }

        protected static List<Lesson> GetLessons(XmlNode day, int week_id, int day_id, string groupName, List<TeacherSchedule> TeacherSchedule)
        {
            var lessons = new List<Lesson>();
            foreach (XmlNode lesson in day)
            {
                var lesson_id = int.Parse(lesson.Attributes!["number"]!.Value);
                lessons.Add(new Lesson(lesson_id, GetSubgroups(lesson, week_id, day_id, groupName, lesson_id, TeacherSchedule)));
            }
             return lessons;
        }

        protected static List<Subgroup> GetSubgroups(XmlNode lesson, int week_id, int day_id, string groupName, int lesson_id, List<TeacherSchedule> TeacherSchedule)
        {
            var subgroups = new List<Subgroup>();
            foreach (XmlNode subgroup in lesson)
            {
                var id = int.Parse(subgroup.Attributes!["number"]!.Value);
                var subject = subgroup.Attributes!["subject"]!.Value;
                var short_subject = subgroup.Attributes!["short_subject"]!.Value;
                var teacherFullName = subgroup.Attributes!["teacher"]!.Value;
                var cabinet = subgroup.Attributes!["cabinet"]!.Value;
                subgroups.Add(new Subgroup(id, subject, short_subject, teacherFullName, cabinet));

                bool Have = false;
                var teacher = TeacherSchedule.FirstOrDefault(e => e.FullName.ToLower() == teacherFullName.ToLower());
                if (teacher is not null)
                    Have = true;
                else
                    teacher = new TeacherSchedule();
                teacher.FullName = teacherFullName;
                teacher.weeks[week_id].days[day_id].lessons.Add(new TeacherLesson(lesson_id, short_subject, groupName, cabinet));
                if(!Have)
                    TeacherSchedule.Add(teacher);
            }
            return subgroups;
        }

        protected static async Task<string>? LoadXml(string filename)
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


        protected static async Task<string> ReadFile(string path)
        {
            try
            {
                using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using StreamReader reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251));


                return await reader_encoding.ReadToEndAsync();
            }
            catch
            {
                await Task.Delay(2000);
                using FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read);
                using StreamReader reader_encoding = new StreamReader(fs, CodePagesEncodingProvider.Instance.GetEncoding(1251));
                return await reader_encoding.ReadToEndAsync();
            }
        }

        protected static List<string> GetLessonsTime(XmlDocument doc)
        {
            var rings = doc.GetElementsByTagName("rings").Item(0);
            var lessons_time = new List<string>();
            foreach (XmlNode ring in rings)
                lessons_time.Add($"{ring.Attributes["begin_time"]!.Value} - {ring.Attributes["end_time"]!.Value}");
            return lessons_time;
        }
        public static List<Group> GetBuilding(int i)
        {
            switch (i)
            {
                case 1: return ul_lenina_24;
                case 2: return ul_b_khmelnickogo_281a;
                case 3: return pr_kosmicheskij_14a;
                case 4: return ul_volkhovstroya_5;
            }
            return new List<Group>();
        }
        public static List<TeacherSchedule> GetTeacherScheduleListByBuildingId(int i)
        {
            switch (i)
            {
                case 1: return teachers_ul_lenina_24;
                case 2: return teachers_ul_b_khmelnickogo_281a;
                case 3: return teachers_pr_kosmicheskij_14a;
                case 4: return teachers_ul_volkhovstroya_5;
            }
            return new List<TeacherSchedule>();
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
        public static List<TeacherSchedule> GetTeacherScheduleByBuilding(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            switch (name)
            {
                case "ul_lenina_24": return teachers_ul_lenina_24;
                case "ul_b_khmelnickogo_281a": return teachers_ul_b_khmelnickogo_281a;
                case "pr_kosmicheskij_14a": return teachers_pr_kosmicheskij_14a;
                case "ul_volkhovstroya_5": return teachers_ul_volkhovstroya_5;
            }
            return new List<TeacherSchedule>();
        }
        public static string GetFilenameByBuilding(string name)
        {
            if (string.IsNullOrEmpty(name))
                return null;

            switch (name)
            {
                case "ul_lenina_24": return "b1";
                case "ul_b_khmelnickogo_281a": return "b2";
                case "pr_kosmicheskij_14a": return "b3";
                case "ul_volkhovstroya_5": return "b4";
            }
            return null;
        }



    }
}

