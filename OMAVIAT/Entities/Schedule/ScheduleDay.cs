using OMAVIAT.Entities.Enums;

namespace OMAVIAT.Entities.Schedule
{
	public class ScheduleDay
	{
		public int Type { get; set; }
		public List<ScheduleLesson> lessons { get; set; } = [];
	}
}
