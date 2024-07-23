using OMAVIAT.Entities.Enums;

namespace OMAVIAT.Entities.Schedule
{
	public class ScheduleDay
	{
		public ScheduleDayType Type { get; set; }
		public List<ScheduleLesson> lessons { get; set; } = [];
	}
}
