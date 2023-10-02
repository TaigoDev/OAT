using Ganss.Excel;
using System.Diagnostics;
using System.Globalization;

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

            var sheets = new ExcelMapper(xlsx).FetchSheetNames().ToList();
            var excel = new ExcelMapper()
            {
                HeaderRow = true,
                HeaderRowNumber = 0,
                MinRowNumber = 1,
                CreateMissingHeaders = true,
                SkipBlankCells = false,
            };
            var monthName = sheets.FirstOrDefault(e => e.ToLower() == month.ToLower());
            if (!DateTime.TryParseExact(month, "MMMM",CultureInfo.CurrentCulture, DateTimeStyles.None, out var dateTime) || monthName is null)
                return null;
            
            MappingDays(ref excel, DateTime.DaysInMonth(DateTime.Now.Year, dateTime.Month));

            var rawRecords = await excel.FetchAsync<RawRecord>(xlsx, monthName);

            var student = Student.Convert(FullName, rawRecords.ToList());
            stopWatch.Stop();
            Logger.InfoWithoutTelegram($"Оценки пользователя загружены за {stopWatch.ElapsedMilliseconds}ms");
            return student;
        }




        private static void MappingDays(ref ExcelMapper excel, int days)
        {
            for (int i = 1; i <= days + 1; i++)
                excel.AddMapping<RawRecord>(2 + i, e => e.cache);
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
