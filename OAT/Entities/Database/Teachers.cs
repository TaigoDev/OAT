namespace OAT.Entities.Database
{
	[MysqlTable]
	public class Teachers
	{
		public Teachers(int id, string FullName, string email, string phone, string base64_profile, string image_url)
		{
			this.id = id;
			this.FullName = FullName;
			this.email = email;
			this.phone = phone;
			this.base64_profile = base64_profile;
			this.image_url = image_url;
		}
		public Teachers() { }
		public int id { get; set; }
		public string FullName { get; set; }
		public string email { get; set; }
		public string phone { get; set; }
		public string base64_profile { get; set; }
		public string image_url { get; set; }
	}


}
