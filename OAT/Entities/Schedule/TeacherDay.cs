namespace OAT.Entities.Schedule
{
	public class TeacherDay
	{
		public TeacherDay(int id, List<TeacherLesson> lessons)
		{
			this.id = id;
			this.lessons = lessons;
		}
		public TeacherDay(int id)
		{
			this.id = id;
		}

		public int id { get; set; }
		public List<TeacherLesson> lessons = new List<TeacherLesson>();
	}

}
