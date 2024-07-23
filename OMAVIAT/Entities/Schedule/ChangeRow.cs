namespace OAT.Entities.Schedule
{
	public class ChangeRow
	{
		[ColumnEPPlus("A")] public string cours { get; set; }
		[ColumnEPPlus("B")] public string group { get; set; }
		[ColumnEPPlus("G")] public string reason { get; set; }

		[ColumnEPPlus("C")] public string was_couple { get; set; }
		[ColumnEPPlus("D")] public string was_cabinet { get; set; }
		[ColumnEPPlus("E")] public string was_discipline { get; set; }
		[ColumnEPPlus("F")] public string was_teacher { get; set; }
		[ColumnEPPlus("H")] public string couple { get; set; }
		[ColumnEPPlus("I")] public string cabinet { get; set; }
		[ColumnEPPlus("J")] public string discipline { get; set; }
		[ColumnEPPlus("K")] public string teacher { get; set; }
	}
}
