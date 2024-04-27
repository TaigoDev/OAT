using OAT.Entities.Interfaces;
using OAT.Utilities;

namespace OAT.Entities.Old.Database
{
	public class News : INews
	{

		public News() { }

		public News(int id, string date, string title, string short_description, string description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.short_description = short_description;
			this.description = description;
			this.photos = photos;
		}

		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string short_description { get; set; }
		public string description { get; set; }
		public string photos { get; set; }

		public List<string> GetPhotos() =>
			photos.toObject<List<string>>();

	}
}
