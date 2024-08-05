using OMAVIAT.Entities.Database;

namespace OMAVIAT.Entities.Schedule
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

		public bool IsRelevant(ChangesTable row)
		{
			if (row.group != group)
				return false;
			if (row.was_cabinet != was_cabinet)
				return false;
			if (row.was_teacher != was_teacher)
				return false;
			if (row.was_cabinet != was_cabinet)
				return false;
			if (row.reason != reason)
				return false;
			if (row.cabinet != cabinet)
				return false;
			if (row.teacher != teacher)
				return false;
			if (row.discipline != discipline)
				return false;
			if (!int.TryParse(couple, null, out var couple_id) || row.couple != couple_id)
				return false;
			if (!int.TryParse(was_couple, null, out var was_couple_id) || row.was_couple != was_couple_id)
				return false;
			return true;
		}
	}
}
