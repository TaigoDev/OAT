using OAT.Utilities;

namespace OAT.Entities.Database
{
	[MysqlTable]
	public class News
	{
		public News(int id, string date, string title, string description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.photos = photos;
		}

		public News(int id, string date, string title, string description, List<string> photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.photos = photos.toJson();
		}

		public News(string date, string title, string description, List<string> photos)
		{
			id = DataBaseUtils.getLastId("News").GetAwaiter().GetResult();
			this.date = date;
			this.title = title;
			this.description = description;
			this.photos = photos.toJson();
		}
		public News() { }
		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string photos { get; set; }


		public List<string> GetPhotos() =>
			photos.toObject<List<string>>();

	}
}
