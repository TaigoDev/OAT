using System.Xml;

namespace OAT.Readers
{
    public class ScheduleReader
    {

        public static void init()
        {
            var xDoc = new XmlDocument();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "schedule.xml");
            if(!File.Exists(path))
            {
                Logger.Error("Файл schedule.xml не найден!");
                return;
            }
            var content = File.ReadAllText(path);
            xDoc.LoadXml(content.Replace("windows-1251", "utf-8"));
            XmlNode groups = xDoc.GetElementsByTagName("timetable").Item(0)!;
            foreach(var group in groups)
            {
                Console.WriteLine(group);
            }
        }

    }
}
