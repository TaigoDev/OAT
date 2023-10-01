using Ganss.Excel;
using System.Diagnostics;

namespace OAT.Readers
{
    public class EvaluationsReader
    {
        //TODO: Проверить возможность одновременного чтения
        public static async Task<Student?> Search(string group, string FullName, string month)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            var xlsx = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "journal", $"{group}.xlsx");
            if(!File.Exists(xlsx))
                return null;

            using var stream = File.Open(xlsx, FileMode.Open, FileAccess.Read);
            var excel = new ExcelMapper()
            {
                HeaderRow = true,
                HeaderRowNumber = 0,
                MinRowNumber = 1,
                CreateMissingHeaders = true,
                SkipBlankCells = false,
            };

            MappingDays(ref excel, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.ParseExact(month, "MMMM", System.Globalization.CultureInfo.CurrentCulture).Month));
            var rawRecords = await excel.FetchAsync<RawRecord>(stream, month);

            stream.Close();
            await stream.DisposeAsync();

            var student = Student.Convert(FullName, rawRecords.ToList());
            stopWatch.Stop();
            Logger.InfoWithoutTelegram($"Оценки пользователя загружены за {stopWatch.ElapsedMilliseconds}ms");
            return student;
        }

        private static void MappingDays(ref ExcelMapper excel, int days)
        {
            for (int i = 0; i < days; i++)
                excel.AddMapping<RawRecord>(3 + i, e => e.cache);
        }
        

        public class RawRecord
        {
            [Column("Студент")]
            public string FullName { get; set; }

            [Column("Дисциплина")]
            public string discipline { get; set; }

            public List<string> marks = new List<string>();

            public string? cache { get { return null;} set { marks.Add(value ?? ""); } }
        }

       
    }
}
