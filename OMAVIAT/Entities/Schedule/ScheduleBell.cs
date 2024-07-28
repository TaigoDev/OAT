namespace OMAVIAT.Entities.Schedule
{
	public class ScheduleBell
	{
		public int Id { get; set; }
		public required string start { get; set; }
		public required string end { get; set; }
	}
}
