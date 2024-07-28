using OMAVIAT.Entities.Enums;

namespace OMAVIAT.Entities.Schedule
{
	public class ScheduleWeek
	{
		public ScheduleWeekType Type { get; set; }
	    public List<ScheduleDay> Days { get; set; }
	}
}
