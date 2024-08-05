using OMAVIAT.Entities.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAVIAT.Entities.Database
{
	public class ChangesTable
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public DateOnly date { get; set; }
		public Corpus corpus { get; set; }
		public string? group { get; set; }
		public string? was_cabinet { get; set; }
		public int? was_couple { get; set; }
		public string? was_teacher { get; set; }
		public string? was_discipline { get; set; }
		public string? reason { get; set; }
		public int? couple { get; set; }
		public string? cabinet { get; set; }
		public string? teacher { get; set; }
		public string? discipline { get; set; }

	}
}
