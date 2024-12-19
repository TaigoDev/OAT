using OMAVIAT.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAVIAT.Entities.Database {
	public class DemoExamNews : INews {

		public DemoExamNews(string date, string title, string description, string short_description, List<string> photos, bool IsFixed)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos.toJson();
			this.IsFixed = IsFixed;
		}
		public DemoExamNews(int id, string date, string title, string description, string short_description, string photos, bool IsFixed)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
			this.IsFixed = IsFixed;
		}

		public DemoExamNews(string date, string title, string description, string short_description, string photos, bool IsFixed)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
			this.IsFixed = IsFixed;
		}
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string short_description { get; set; }
		public string photos { get; set; }
		public bool IsFixed { get; set; }


		public List<string> GetPhotos() =>
			photos.toObject<List<string>>();
	}
}
