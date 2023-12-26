using Ganss.Excel;
using NPOI.SS.Formula.Functions;
using OAT.Utilities;
using OfficeOpenXml;
using Org.BouncyCastle.Crypto;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using static OAT.Readers.ChangesController;


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

            var ids = new[] { 1, 2, 3, 4};
            await Runs<int>.InTasks(UpdateCorpusChanges, ids.ToList(), false);
            stopWatch.Stop();
            Logger.Info($"Изменения загружены за {stopWatch.ElapsedMilliseconds} ms");    
        }


        public static Task GetTask(int index) =>
            new Task(async () =>
                {
                    try
                    {
                        await UpdateCorpusChanges(index);
                    }
                    catch (Exception ex)
                    {
                        Logger.Error($"Произошла ошибка при загрузке корпус {index}.\n {ex}");
                    }
                });

        public static async Task UpdateCorpusChanges(int index, bool IsRepeat = false)
        {
            var xlsx = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{index}-changes.xlsx");
            if (!File.Exists(xlsx))
                return;

            if (FileUtils.IsFileLocked(xlsx) && !IsRepeat)
            {
                RepeaterUtils.Try(() => UpdateCorpusChanges(index, true), TimeSpan.FromMinutes(10), 3);
                return;
            }

           await using FileStream fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
            var excel = new ExcelPackage(fileStream);
            var corpus = GetListChanges(index);
            corpus.Clear();
            var sheets = excel.Workbook.Worksheets.ToList();
            foreach (var sheet in sheets.Count() >= 3 ? sheets.GetRange(sheets.Count() - 3, 3) : sheets.GetRange(0, sheets.Count()))
            {
                try
                {
                    var changes = GetChangesAsync(excel, sheet.Name);
                    var bells = GetBellsAsync(excel, sheet.Name);
                    if (changes != null)
                        corpus.Add(new(sheet.Name, bells, changes.Where(e => e.group is not null)));
                }
                catch (Exception ex)
                {
                    Logger.Error($"Произошла ошибка при загрузке листа {sheet} корпус {index}.\n {ex}");
                }
            }
        }

        public static IEnumerable<ChangeRow> GetChangesAsync(ExcelPackage excel, string? sheet = null)
        {
            if (sheet is null)
                sheet = excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
            var workSheet = excel.Workbook.Worksheets[sheet];
            var newcollection = workSheet.Fetch<ChangeRow>(11, 300);
            return newcollection;
        }

        public static IEnumerable<Bell> GetBellsAsync(ExcelPackage excel, string? sheet = null)
        {
            if (sheet is null)
                sheet = excel.Workbook.Worksheets!.Max(e => DateTime.ParseExact(e.Name, "d.MM", null)).ToString("dd.MM");
            var workSheet = excel.Workbook.Worksheets[sheet];
            var newcollection = workSheet.Fetch<Bell>(11, 16);
            return newcollection;
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



        [AttributeUsage(AttributeTargets.All)]
        public class ColumnEPPlus : System.Attribute
        {
            public int ColumnIndex { get; set; }


            public ColumnEPPlus(int column)
            {
                ColumnIndex = column;
            }

            public ColumnEPPlus(string letter)
            {
                ColumnIndex = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter) + 1;
            }
        }

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
        [ColumnEPPlus("M")] public string id { get; set; }
        [ColumnEPPlus("N")] public string period { get; set; }
    }
    public static class EPPLusExtensions
    {

        public static IEnumerable<T> Fetch<T>(this ExcelWorksheet worksheet, int start = 0, int end = 300) where T : new()
        {
           
            Func<CustomAttributeData, bool> columnOnly = y => y.AttributeType == typeof(ColumnEPPlus);

            var columns = typeof(T)
                    .GetProperties()
                    .Where(x => x.CustomAttributes.Any(columnOnly))
            .Select(p => new
            {
                Property = p,
                Column = p.GetCustomAttributes<ColumnEPPlus>().First().ColumnIndex //safe because if where above
            }).ToList();


            var rows = worksheet.Cells
                .Select(cell => cell.Start.Row)
                .Distinct()
                .OrderBy(x => x);

            var collection = rows.Where(e => e >= start && e <= end)
                .Select(row =>
                {
                    var tnew = new T();
                    columns.ForEach(col =>
                    {
                        var val = worksheet.Cells[row, col.Column];
                        if (val.Value == null)
                        {
                            col.Property.SetValue(tnew, null);
                            return;
                        }
                        if (col.Property.PropertyType == typeof(Int32))
                        {
                            col.Property.SetValue(tnew, val.GetValue<int>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(double))
                        {
                            col.Property.SetValue(tnew, val.GetValue<double>());
                            return;
                        }
                        if (col.Property.PropertyType == typeof(DateTime))
                        {
                            col.Property.SetValue(tnew, val.GetValue<DateTime>());
                            return;
                        }
                        col.Property.SetValue(tnew, val.GetValue<string>());
                    });

                    return tnew;
                });


            //Send it back
            return collection;
        }
    }
    public class ChangeRow
    {
        [ColumnEPPlus("A")] public string cours { get; set; }
        [ColumnEPPlus("B")] public string group { get; set; }
        [ColumnEPPlus("G")] public string reason { get; set; }

        [ColumnEPPlus("C")] public string was_couple { get; set; }
        [ColumnEPPlus("D")] public string was_cabinet { get; set; }
        [ColumnEPPlus("E")] public string was_discipline { get; set; }
        [ColumnEPPlus("F")] public string was_teacher { get; set; }
        [ColumnEPPlus("H")] public string couple { get; set; }
        [ColumnEPPlus("I")] public string cabinet { get; set; }
        [ColumnEPPlus("J")] public string discipline { get; set; }
        [ColumnEPPlus("K")] public string teacher { get; set; }
    }
}
