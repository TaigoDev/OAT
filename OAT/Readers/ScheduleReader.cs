#pragma warning disable CS8602
using System.Diagnostics;
using System.Text;
using System.Xml;
using static OAT.Utilities.ScheduleUtils;

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
                    XmlNode xml_groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
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
                    lesson_times = GetLessonsTime(xDoc);

                }
                catch (Exception ex)
                {
                    Logger.ErrorWithCatch($"Ошибка загрузки корпуса b{i}. \nОшибка: {ex}");
                }


            }
            IterateTeacherList(e => e.OrderBy(e => e.FullName.ToCharArray()[0]).ToList());
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

       
    }
}

