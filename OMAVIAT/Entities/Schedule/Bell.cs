namespace OMAVIAT.Entities.Schedule
{

	public class Bell
	{
		[ColumnEPPlus("M")] public string id { get; set; }
		[ColumnEPPlus("N")] public string period { get; set; }
	}
}
