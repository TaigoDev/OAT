using Ganss.Excel;
using System.Diagnostics;


namespace OAT.Readers
{
    public class ChangesController
    {
        public static List<Changes> changes_b1 = new List<Changes>();
        public static List<Changes> changes_b2 = new List<Changes>();
        public static List<Changes> changes_b3 = new List<Changes>();
        public static List<Changes> changes_b4 = new List<Changes>();


        public static async Task init()
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 1; i <= 4; i++)
            {
                try
                {
                    var xlsx = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{i}-changes.xlsx");
                    if (!File.Exists(xlsx))
                        continue;

                    var sheets = GetSheetsName(xlsx);
                    var corpus = GetListChanges(i);
                    foreach (var sheet in sheets.Count() >= 3 ? sheets.GetRange(sheets.Count() - 3, 3) : sheets.GetRange(0, sheets.Count()))
                    {
                        try
                        {
                            var changes = await GetChangesAsync(xlsx, sheet);
                            var bells = await GetBellsAsync(xlsx, sheet);
                            if (changes != null)
                                corpus.Add(new(sheet, bells, changes.Where(e => e.group is not null)));
                        }
                        catch (Exception ex)
                        {
                            Logger.Error($"Произошла ошибка при загрузке листа {sheet} корпус {i}.\n {ex}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Logger.Error($"Произошла ошибка при загрузке корпус {i}.\n {ex}");
                }
            }
            stopWatch.Stop();
            Logger.Info($"Изменения загружены за {stopWatch.ElapsedMilliseconds} ms");
        
        
        }


        public static async Task<IEnumerable<ChangeRow>?> GetChangesAsync(string xlsx, string? sheet = null)
        {
            var sheets = sheet is null ? GetSheetsName(xlsx) : null;
            var excel = new ExcelMapper()
            {
                HeaderRow = true,
                HeaderRowNumber = 9,
                MinRowNumber = 10,
                CreateMissingHeaders = true,
                SkipBlankRows = true,
                MaxRowNumber = 300
            };
            if (sheet is null)
                sheet = sheets!.Max(e => DateTime.ParseExact(e, "d.MM", null)).ToString("dd.MM");
            return await excel.FetchAsync<ChangeRow>(xlsx, sheet.Replace(" ", ""));
        }

        public static async Task<IEnumerable<Bell>> GetBellsAsync(string xlsx, string? sheet = null)
        {
            var sheets = sheet is null ? GetSheetsName(xlsx) : null;
            var excel = new ExcelMapper()
            {
                HeaderRow = false,
                MinRowNumber = 10,
                MaxRowNumber = 15,
            };

            if (sheet is null)
                sheet = sheets!.Max(e => DateTime.ParseExact(e, "d.MM", null)).ToString("dd.MM");

            return await excel.FetchAsync<Bell>(xlsx, sheet.Replace(" ", ""));
        }

        public static List<string> GetSheetsName(string xlsx)
        {
            if (!File.Exists(xlsx))
                return new();
            return new ExcelMapper(xlsx).FetchSheetNames().ToList();
        }


        public static List<Changes> GetListChanges(int id) =>
            id switch
            {
                1 => changes_b1,
                2 => changes_b2,
                3 => changes_b3,
                4 => changes_b4,
                _ => new List<Changes>()
            };



    }

    public class Changes
    {
        public Changes(string sheetName, IEnumerable<Bell> bells, IEnumerable<ChangeRow> rows)
        {
            SheetName = sheetName;
            this.bells = bells;
            this.rows = rows;
        }

        public string SheetName { get; set; }
        public IEnumerable<Bell> bells { get; set; }
        public IEnumerable<ChangeRow> rows { get; set; }

    }

    public class Bell
    {
        [Column(Letter = "M")] public string id { get; set; }
        [Column(Letter = "N")] public string period { get; set; }
    }

    public class ChangeRow
    {
        [Column(Letter = "A")] public string cours { get; set; }
        [Column("Группа")] public string group { get; set; }
        [Column(Letter = "G")] public string reason { get; set; }

        [Column(Letter = "C")] public string was_couple { get; set; }
        [Column(Letter = "D")] public string was_cabinet { get; set; }
        [Column(Letter = "E")] public string was_discipline { get; set; }
        [Column(Letter = "F")] public string was_teacher { get; set; }
        [Column(Letter = "H")] public string couple { get; set; }
        [Column(Letter = "I")] public string cabinet { get; set; }
        [Column(Letter = "J")] public string discipline { get; set; }
        [Column(Letter = "K")] public string teacher { get; set; }
    }
}
