using OAT.Utilities;

namespace OAT.Entities.Database
{
	[MysqlTable]
	public class DemoExamNews
	{


		public DemoExamNews(string date, string title, string description, string short_description, List<string> photos)
		{
			id = DataBaseUtils.getLastId("DemoExamNews").GetAwaiter().GetResult();
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos.toJson();
		}
		public DemoExamNews() { }

		public DemoExamNews(int id, string date, string title, string description, string short_description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
		}

		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string short_description { get; set; }
		public string photos { get; set; }


		public List<string> GetPhotos() =>
			photos.toObject<List<string>>();
	}
}
