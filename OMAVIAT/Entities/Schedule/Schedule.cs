using OMAVIAT.Entities.Enums;

namespace OMAVIAT.Entities.Schedule
{
	public class Schedule
	{
		public required ScheduleType type { get; set; }
		public required string name { get; set; }
		public int? Curs { get; set; }
		public required List<ScheduleWeek> Weeks { get; set; }
	}
}
