namespace OMAVIAT.Entities;

[AttributeUsage(AttributeTargets.All)]
public class ColumnEPPlus : Attribute
{
	public ColumnEPPlus(int column)
	{
		ColumnIndex = column;
	}

	public ColumnEPPlus(string letter)
	{
		ColumnIndex = "ABCDEFGHIJKLMNOPQRSTUVWXYZ".IndexOf(letter) + 1;
	}

	public int ColumnIndex { get; set; }
}