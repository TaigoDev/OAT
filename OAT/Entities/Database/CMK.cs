using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata; 

namespace OAT.Entities.Database
{
	public class CMK
	{
		public CMK()
		{
		}

		public CMK(string name, List<string> compositions, string history, List<Documents> documents, List<CMK_News> news)
		{
			this.name = name;
			this.compositions = compositions;
			this.history = history;
			this.documents = documents;
			this.news = news;
		}

		public CMK(int id, string name, List<string> compositions, string history, List<Documents> documents, List<CMK_News> news)
		{
			this.id = id;
			this.name = name;
			this.compositions = compositions;
			this.history = history;
			this.documents = documents;
			this.news = news;
		}

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string name { get; set; } = "ЦМК";
		public List<string> compositions { get; set; } = [];
		public string history { get; set; } = "История ЦМК";
		public virtual IList<Documents> documents { get; set; }
		public virtual IList<CMK_News> news { get; set; }
	}
}
