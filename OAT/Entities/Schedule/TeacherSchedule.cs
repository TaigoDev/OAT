namespace OAT.Entities.Schedule
{

	public class TeacherSchedule
	{
		public TeacherSchedule()
		{
		}

		public TeacherSchedule(string fullName, List<TeacherWeek> weeks)
		{
			FullName = fullName;
			this.weeks = weeks;
		}

		public string FullName { get; set; }
		public List<TeacherWeek> weeks = new List<TeacherWeek>(2)
	{
		new TeacherWeek(1),
		new TeacherWeek(2)
	};

	}
}
