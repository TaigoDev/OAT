namespace OMAVIAT.Entities.Configs
{
	public class Config
	{
		public string BaseUrl { get; set; }
		public string MainUrl { get; set; }
		public int bind_port { get; set; }
		public string db_ip { get; set; }
		public int db_port { get; set; }
		public string db_user { get; set; }
		public string db_password { get; set; }
		public string db_name { get; set; }
		public string ldap_IP { get; set; }
		public int ldap_port { get; set; }
		public string ldap_login { get; set; }
		public string ldap_password { get; set; }
		public string ldap_domain { get; set; }
		public string ldap_zone { get; set; }
		public bool bitrixProxy { get; set; } 
		public string redis_ip { get; set; } = "PASTE REDIS IP";
		public int redis_port { get; set; } = 0;
		public string redis_password { get; set; } = "PASTE REDIS PASSWORD";
	}
}
