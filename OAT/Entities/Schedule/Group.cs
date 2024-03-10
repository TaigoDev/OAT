namespace OAT.Entities.Schedule
{
	public class Group
	{
		public Group(string name, int curse, List<Week> weeks)
		{
			this.name = name;
			this.weeks = weeks;
			this.curse = curse;
		}

		public string name { get; set; }
		public int curse { get; set; }
		public List<Week> weeks { get; set; } = new List<Week>(2);
	}
}
