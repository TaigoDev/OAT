using static OMAVIAT.Controllers.Evalutions.EvaluationsReader;

namespace OMAVIAT.Entities.Journal;

public class Student
{
	public List<Discipline> Disciplines = new();

	public Student(string fullName)
	{
		FullName = fullName;
	}

	public string FullName { get; set; }

	public static List<Student> ConvertAll(List<RawRecord> records)
	{
		var students = new List<Student>();
		var FullNames = records.GroupBy(x => x.FullName).Select(d => d.First()).ToList().ConvertAll(e => e.FullName);
		foreach (var FullName in FullNames)
			students.Add(Convert(FullName, records));
		return students;
	}

	public static Student Convert(string FullName, List<RawRecord> records)
	{
		var rows = records.Where(e => e.FullName.Replace("ё", "е") == FullName.Replace("ё", "е"));
		var student = new Student(FullName);
		foreach (var discipline in rows)
			student.Disciplines.Add(new Discipline(discipline.Discipline, discipline.Marks));
		return student;
	}
}