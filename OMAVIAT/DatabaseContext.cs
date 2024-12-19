using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OMAVIAT.Entities.Database;
using OMAVIAT.Entities.Schedule;
using OMAVIAT.Entities.Schedule.Database;
using OMAVIAT.Schedule;

namespace OMAVIAT {
	public class DatabaseContext : ExcelContext {
		public virtual DbSet<Tokens> Tokens { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<ProfNews> ProfNews { get; set; }
		public virtual DbSet<DemoExamNews> DemoExamNews { get; set; }
		public virtual DbSet<IPTables> IPTables { get; set; }
		public virtual DbSet<Documents> documents { get; set; }
		public virtual DbSet<CMK> CMK { get; set; }


	}
}
