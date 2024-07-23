namespace OAT.Entities.Journal
{
	public class Discipline
	{
		public string discipline { get; set; }
		public List<string> marks = new List<string>();

		public Discipline(string discipline, List<string> marks)
		{
			this.discipline = discipline;
			this.marks = marks;
		}
	}

}
