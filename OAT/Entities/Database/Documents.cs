using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAT.Entities.Database
{
	public class Documents(string name, string url)
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string name { get; set; } = name;
		public string url { get; set; } = url;
	}
}
