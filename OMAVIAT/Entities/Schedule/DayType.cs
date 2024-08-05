namespace OMAVIAT.Entities.Schedule
{
	public class DayType
	{
		public DayType()
		{
		}

		public DayType(string type)
		{
			this.type = type;
		}

		[ColumnEPPlus("N")] public string type { get; set; }
	}
}
