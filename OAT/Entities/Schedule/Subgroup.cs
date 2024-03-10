namespace OAT.Entities.Schedule
{
	public class Subgroup
	{
		public Subgroup(int id, string subject, string short_subject, string teacher, string cabinet)
		{
			this.id = id;
			this.subject = subject;
			this.short_subject = short_subject;
			this.teacher = teacher;
			this.cabinet = cabinet;
		}

		public int id { get; set; }
		public string subject { get; set; }
		public string short_subject { get; set; }
		public string teacher { get; set; }
		public string cabinet { get; set; }
	}
}
