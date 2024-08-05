using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAVIAT.Entities.Database
{
	public class Tokens(string username, string token, string issued, string Roles)
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string username { get; set; } = username;
		public string Token { get; set; } = token;
		public string Roles { get; set; } = Roles;
		public string issued { get; set; } = issued;
	}

}
