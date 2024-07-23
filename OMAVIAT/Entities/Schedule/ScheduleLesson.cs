namespace OMAVIAT.Entities.Schedule
{
	public class ScheduleLesson
	{
		public int Id { get; set; }
		public required string Name { get; set; }
		public required string Group { get; set; }
		public required string Teacher { get; set; }
		public required string Cabinet { get; set; }
	}
}
