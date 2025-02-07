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
	
	public List<string> GetPhotos()
	{
		return photos.toObject<List<string>>();
	}

	public MuseumHistoryNews()
	{
		
	}
	
	public MuseumHistoryNews(string date, string title, string description, string short_description, List<string> photos,
		bool IsFixed)
	{
		id = id;
		this.date = date;
		this.title = title;
		this.short_description = short_description;
		this.description = description;
		this.photos = photos.toJson();
		this.IsFixed = IsFixed;
	}
}