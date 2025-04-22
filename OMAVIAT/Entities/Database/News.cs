using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using OMAVIAT.Entities.Interfaces;

namespace OMAVIAT.Entities.Database;

public class News : INews
{
	public News(int id, string date, string title, string short_description, string description, string photos,
		bool IsFixed)
	{
		this.id = id;
		this.date = date;
		this.title = title;
		this.short_description = short_description;
		this.description = description;
		this.photos = photos;
		this.IsFixed = IsFixed;
	}

	public News(string date, string title, string description, string short_description, List<string> photos,
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

	public News(string date, string title, string description, string short_description, string photos, bool IsFixed)
	{
		id = id;
		this.date = date;
		this.title = title;
		this.short_description = short_description;
		this.description = description;
		this.photos = photos;
		this.IsFixed = IsFixed;
	}

	[Key]
	[DatabaseGenerated(DatabaseGeneratedOption.Identity)]

	public int id { get; set; }

	public string date { get; set; }
	public string title { get; set; }
	public string short_description { get; set; }
	public string description { get; set; }
	public string photos { get; set; }
	public bool IsFixed { get; set; }
	public bool IsHide { get; set; }
	public int? EloTelegramMessageId { get; set; }
	public int? TelegramMessageId { get; set; }
	public string? TelegramMediaGroupId { get; set; }
	public string? EloTelegramMediaGroupId { get; set; }
	public long EloVkPostId { get; set; }
	public long VkPostId { get; set; }

	public List<string> GetPhotos()
	{
		return photos.toObject<List<string>>();
	}
}