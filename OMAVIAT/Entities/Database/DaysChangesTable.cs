using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAVIAT.Entities.Database
{
	public class DaysChangesTable
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public DateOnly date { get; set; }
		public Corpus corpus { get; set; }
		public string? type { get; set; }
		public string? DateText { get; set; }
		public string? SchoolWeek { get; set; }
		public  List<Bell> bells { get; set; } = [];


		public bool IsRelevant(string? type, List<Bell> bells) => this.type != type || this.bells != bells;
	}
}
