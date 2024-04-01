using OAT.Utilities;

namespace OAT.Entities.Database
{
	[MysqlTable]
	public class IPTables
	{
		public IPTables()
		{
		}

		public IPTables(string iP, int attempts, DateTime lastFailAttempt, DateTime banTime)
		{
			id = DataBaseUtils.getLastId("IPTables").GetAwaiter().GetResult();
			IP = iP;
			this.attempts = attempts;
			LastFailAttempt = lastFailAttempt;
			BanTime = banTime;
		}

		public IPTables(int id, string iP, int attempts, DateTime lastFailAttempt, DateTime banTime)
		{
			this.id = id;
			IP = iP;
			this.attempts = attempts;
			LastFailAttempt = lastFailAttempt;
			BanTime = banTime;
		}

		public int id { get; set; }
		public string IP { get; set; }
		public int attempts { get; set; }
		public DateTime LastFailAttempt { get; set; }
		public DateTime BanTime { get; set; }
	}
}
