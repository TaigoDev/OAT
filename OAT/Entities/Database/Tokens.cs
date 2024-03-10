using OAT.Utilities;

namespace OAT.Entities.Database
{
	[MysqlTable]
	public class Tokens
	{

		public Tokens() { }

		public Tokens(int id, string username, string token, string issued)
		{
			this.id = id;
			this.username = username;
			Token = token;
			this.issued = issued;
		}
		public Tokens(string username, string token, string issued, string Roles)
		{
			id = DataBaseUtils.getLastId("Tokens").GetAwaiter().GetResult(); ;
			this.username = username;
			Token = token;
			this.issued = issued;
			this.Roles = Roles;
		}
		public int id { get; set; }
		public string username { get; set; }
		public string Token { get; set; }
		public string Roles { get; set; }
		public string issued { get; set; }
	}

}
