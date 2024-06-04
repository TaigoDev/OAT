namespace OAT.Entities.Schedule
{

	public class Changes(string sheetName, string SchoolWeek, string Date, IEnumerable<Bell> bells, IEnumerable<ChangeRow> rows)
	{
		public string SheetName { get; set; } = sheetName;
		public string SchoolWeek { get; set; } = SchoolWeek;
		public string Date { get; set; } = Date;
		public IEnumerable<Bell> bells { get; set; } = bells;
		public IEnumerable<ChangeRow> rows { get; set; } = rows;

	}
}
