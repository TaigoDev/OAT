namespace OAT.Entities.Schedule
{
	public class Week
	{
		public Week(int id, List<Day> days)
		{
			this.id = id;
			this.days = days;
		}

		public int id { get; set; }
		public List<Day> days { get; set; } = new List<Day>(7);
	}
}
