using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OAT.Entities.Database;

namespace OAT
{
	public class DatabaseContext : DbContext
	{
		public virtual DbSet<Tokens> Tokens { get; set; }
		public virtual DbSet<News> News { get; set; }
		public virtual DbSet<ProfNews> ProfNews { get; set; }
		public virtual DbSet<DemoExamNews> DemoExamNews { get; set; }
		public virtual DbSet<IPTables> IPTables { get; set; }
		public virtual DbSet<Documents> documents { get; set; }
		public virtual DbSet<CMK> CMK { get; set; }
		public virtual DbSet<CMK_News> CMK_News { get; set; }



		public DatabaseContext() : base(GetOptions())
		{
			Database.EnsureCreated();
		}

		public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
		{
			Database.EnsureCreated();
		}


		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);
		}

		private static DbContextOptions GetOptions()
		{
			var options = new DbContextOptionsBuilder();
			options.UseLazyLoadingProxies();
			options.ConfigureWarnings(builder => builder.Ignore(CoreEventId.LazyLoadOnDisposedContextWarning));
			if (OperatingSystem.IsWindows())
				return SqliteDbContextOptionsBuilderExtensions.UseSqlite(options, "Data Source=database.db").Options;

			var builder = MySqlDbContextOptionsBuilderExtensions.UseMySql(options, $"server={Configurator.config.db_ip};port=3306;user={Configurator.config.db_user};password={Configurator.config.db_password};database={Configurator.config.db_name};", new MySqlServerVersion(new Version(8, 0, 25)));
			return builder.Options;
		}
	}
}
