namespace OMAVIAT.Entities.Journal;

public class Discipline
{
	public List<string> marks = new();

	public Discipline(string discipline, List<string> marks)
	{
		this.discipline = discipline;
		this.marks = marks;
	}

	public string discipline { get; set; }
}