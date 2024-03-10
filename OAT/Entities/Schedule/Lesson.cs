namespace OAT.Entities.Schedule
{
	public class Lesson
	{
		public Lesson(int id, List<Subgroup> subgroups)
		{
			this.id = id;
			this.subgroups = subgroups;
		}

		public int id { get; set; }
		public List<Subgroup> subgroups { get; set; } = new List<Subgroup>();
	}

}
