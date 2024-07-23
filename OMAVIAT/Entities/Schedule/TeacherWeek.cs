namespace OAT.Entities.Schedule
{
	public class TeacherWeek
	{
		public TeacherWeek(int id, List<TeacherDay> days)
		{
			this.id = id;
			this.days = days;
		}
		public TeacherWeek(int id)
		{
			this.id = id;
		}
		public int id { get; set; }
		public List<TeacherDay> days = new List<TeacherDay>(7)
	{
		new TeacherDay(1),
		new TeacherDay(2),
		new TeacherDay(3),
		new TeacherDay(4),
		new TeacherDay(5),
		new TeacherDay(6),
		new TeacherDay(7),
	};
	}
}
