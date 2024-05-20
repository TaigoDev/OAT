using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using OAT.Entities.Database;
using OAT.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OAT.Merge
{
	public class OldDatabaseContext : DbContext
	{
		public virtual DbSet<Tokens> Tokens { get; set; }
		public virtual DbSet<Merge.News> News { get; set; }
		public virtual DbSet<Merge.ProfNews> ProfNews { get; set; }
		public virtual DbSet<Merge.DemoExamNews> DemoExamNews { get; set; }
		public virtual DbSet<IPTables> IPTables { get; set; }
		public virtual DbSet<Documents> documents { get; set; }
		public virtual DbSet<CMK> CMK { get; set; }
		public virtual DbSet<CMK_News> CMK_News { get; set; }



		public OldDatabaseContext() : base(GetOptions())
		{
			Database.EnsureCreated();
		}

		public OldDatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
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
			return SqliteDbContextOptionsBuilderExtensions.UseSqlite(options, "Data Source=database.db").Options;
		}
	}

	public class DemoExamNews
	{

		public DemoExamNews(string date, string title, string description, string short_description, List<string> photos)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos.toJson();
		}
		public DemoExamNews(int id, string date, string title, string description, string short_description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
		}

		public DemoExamNews(string date, string title, string description, string short_description, string photos)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
		}
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string short_description { get; set; }
		public string photos { get; set; }

	}

	public class ProfNews 
	{
		public ProfNews(int id, string date, string title, string description, string short_description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
		}
		public ProfNews(int id, string date, string title, string description, string short_description, List<string> photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos.toJson();
		}
		public ProfNews(string date, string title, string description, string short_description, List<string> photos)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos.toJson();
		}
		public ProfNews(string date, string title, string description, string short_description, string photos)
		{
			this.date = date;
			this.title = title;
			this.description = description;
			this.short_description = short_description;
			this.photos = photos;
		}

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string description { get; set; }
		public string short_description { get; set; }
		public string photos { get; set; }

	}

	public class News
	{

		public News(int id, string date, string title, string short_description, string description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.short_description = short_description;
			this.description = description;
			this.photos = photos;
		}

		public News(string date, string title, string description, string short_description, List<string> photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.short_description = short_description;
			this.description = description;
			this.photos = photos.toJson();
		}

		public News(string date, string title, string description, string short_description, string photos)
		{
			this.id = id;
			this.date = date;
			this.title = title;
			this.short_description = short_description;
			this.description = description;
			this.photos = photos;
		}
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]

		public int id { get; set; }
		public string date { get; set; }
		public string title { get; set; }
		public string short_description { get; set; }
		public string description { get; set; }
		public string photos { get; set; }

	}
}
