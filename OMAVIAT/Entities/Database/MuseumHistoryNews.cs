using OMAVIAT.Entities.Interfaces;

namespace OMAVIAT.Entities.Database;

public class MuseumHistoryNews : INews
{
	public int id { get; set; }
	public string date { get; set; }
	public string title { get; set; }
	public string description { get; set; }
	public string short_description { get; set; }
	public string photos { get; set; }
	public bool IsFixed { get; set; }
}