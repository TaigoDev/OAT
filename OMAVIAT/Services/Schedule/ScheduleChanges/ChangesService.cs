using Microsoft.EntityFrameworkCore;
using NPOI.SS.UserModel;
using OfficeOpenXml;
using OMAVIAT.Entities.Database;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Utilities;

namespace OMAVIAT.Services.Schedule.ScheduleChanges
{
	public class ChangesService
	{
		public static async Task InitAsync()
		{
			var ids = new[] { 1, 2, 3, 4 };
			await Runs<int>.InTasks(LoadChangesAsync, [.. ids], false);
		}

		public static async Task LoadChangesAsync(int index, bool IsRepeat = false)
		{
			try
			{
				var xlsx = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "schedule", $"b{index}-changes.xlsx");
				if (!File.Exists(xlsx))
					return;

				if (FileUtils.IsFileLocked(xlsx) && !IsRepeat)
				{
					RepeaterUtils.Try(() => LoadChangesAsync(index, true), TimeSpan.FromMinutes(10), 3);
					return;
				}

				await using var fileStream = new FileStream(xlsx, FileMode.Open, FileAccess.Read);
				var excel = new ExcelPackage(fileStream);
				var sheets = excel.Workbook.Worksheets.ToList();
				sheets.Reverse();
				foreach (var sheet in sheets)
				{
					try
					{
						var changes = ChangesReader.GetChangesAsync(excel, sheet.Name);
						var bells = ChangesReader.GetBellsAsync(excel, sheet.Name);
						if (changes != null && DateTime.ParseExact(sheet.Name, "dd.MM", null).AddDays(7) > DateTime.Now)
						{
							var dayChanges = new Changes()
							{
								bells = bells.ToList(),
								dayType = ChangesReader.GetDayType(excel, sheet.Name),
								Date = ChangesReader.GetDateText(excel, sheet.Name),
								corpus = Enum.Parse<Corpus>($"b{index}"),
								rows = changes.Where(e => e.group is not null).ToList(),
								SchoolWeek = ChangesReader.GetSchoolWeek(excel, sheet.Name),
								SheetName = sheet.Name,
							};
							await InitializeСhanges((Corpus)index - 1, dayChanges);
						}
					}
					catch (Exception ex)
					{
						Logger.Error($"Произошла ошибка при загрузке листа {sheet} корпус {index}.\n {ex}");
					}
				}
			}
			catch(Exception ex)
			{
				Logger.Error($"Произошла ошибка при загрузке корпуса {index}.\n {ex}");

			}
		}

		public static async Task InitializeСhanges(Corpus corpus, Changes changes)
		{
			try
			{
				using var db = new DatabaseContext();
				var date = DateOnly.ParseExact(changes.SheetName, "dd.MM");
				var daychanges = await db.daysChanges.FirstOrDefaultAsync(e => e.corpus == corpus && e.date == date);


				if (daychanges is not null && daychanges.IsRelevant(changes.dayType, changes.bells))
				{
					db.Remove(daychanges);
					await db.SaveChangesAsync();
					daychanges = null;
				}

				if (daychanges is null)
				{
					daychanges = new()
					{
						date = date,
						corpus = corpus,
						bells = changes.bells,
						type = changes.dayType,
						SchoolWeek = changes.SchoolWeek,
						DateText = changes.Date,
					};
					await db.AddAsync(daychanges);
				}
				var deleted_from_db = 0;
				var deleted_from_excel = 0;
				var rows = changes.rows.Where(e => e.group != null).ToList();
				var db_rows = db.changes.Where(e => e.date == date && e.corpus == corpus).AsAsyncEnumerable();
				await foreach (var row in db_rows)
				{
					var record = rows.FirstOrDefault(e => e.IsRelevant(row));
					if (record is null)
					{
						db.Remove(row);
						deleted_from_db++;
					}
					else
					{
						rows.Remove(record);
						deleted_from_excel++;
					}
				}

				var pasted = rows.ConvertAll(e => new ChangesTable()
				{
					date = date,
					corpus = corpus,
					group = e.group,
					was_cabinet = e.was_cabinet,
					was_couple = IntUtils.ToInt32(e.was_couple),
					was_teacher = e.was_teacher,
					was_discipline = e.was_discipline,
					reason = e.reason,
					couple = IntUtils.ToInt32(e.couple),
					cabinet = e.cabinet,
					discipline = e.discipline,
					teacher = e.teacher,
				});
				await db.AddRangeAsync(pasted);
				await db.SaveChangesAsync();
				if (pasted.Count != 0 || deleted_from_db != 0)
					Logger.Info($"Изменения загружены для корпуса {corpus} из листа {changes.SheetName} загружены. Всего строк: {pasted.Count}! Удалено из бд: {deleted_from_db}. Удалено из списка: {deleted_from_excel}");
			}
			catch (Exception ex)
			{
				Logger.Error(ex);
			}
		}

		private static void GarbageCollector()
		{
			new Task(async () =>
			{
				try
				{
					while (true)
					{
						using var db = new DatabaseContext();
						var date = DateOnly.FromDateTime(DateTime.Now).AddDays(-7);
						db.RemoveRange(db.changes.Where(e => e.date < date));
						db.RemoveRange(db.daysChanges.Where(e => e.date < date));
						await db.SaveChangesAsync();
						await Task.Delay(TimeSpan.FromDays(1));
					}
				}
				catch (Exception ex)
				{
					Logger.Error(ex);
				}
			}).Start();
		}
	}
}
