namespace OMAVIAT.Entities;

public class IPage
{
	public IPage(string map, string current)
	{
		this.current = current;
		this.map = map;
	}

	public string current { get; set; }
	public string map { get; set; }
}