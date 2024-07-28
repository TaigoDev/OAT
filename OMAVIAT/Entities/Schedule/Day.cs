namespace OAT.Entities.Schedule
{
	public class Day
	{
		public Day(string name, List<Lesson> lessons)
		{
			this.name = name;
			this.lessons = lessons;
		}

		public string name { get; set; }
		public List<Lesson> lessons { get; set; } = new List<Lesson>();
	}
}
