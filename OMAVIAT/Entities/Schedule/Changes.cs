using OMAVIAT.Entities.Enums;

namespace OMAVIAT.Entities.Schedule
{

	public class Changes
	{
		public required Corpus corpus { get; set; }
		public required string SheetName { get; set; }
		public required string SchoolWeek { get; set; }
		public required string Date { get; set; }
		public required string dayType { get; set; }
		public required List<Bell> bells { get; set; }
		public required List<ChangeRow> rows { get; set; }

	}
}
