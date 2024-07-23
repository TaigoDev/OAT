namespace OAT.Entities.Schedule
{
	public class TeacherLesson
	{
		public TeacherLesson()
		{
		}

		public TeacherLesson(int id, string short_subject, string group, string cabinet)
		{
			this.id = id;
			this.short_subject = short_subject;
			this.group = group;
			this.cabinet = cabinet;
		}

		public int id { get; set; }
		public string short_subject { get; set; }
		public string group { get; set; }
		public string cabinet { get; set; }
	}
}
