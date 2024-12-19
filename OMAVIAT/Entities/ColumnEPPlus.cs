namespace OMAVIAT.Entities {
	[AttributeUsage(AttributeTargets.All)]
	public class ColumnEPPlus : Attribute {
		public int ColumnIndex { get; set; }


		public ColumnEPPlus(int column)
		{
			ColumnIndex = column;
		}

		public ColumnEPPlus(string letter)
		{
			ColumnIndex = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter) + 1;
		}
	}
}
