using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OMAVIAT.Entities.Database
{
	public class CMK
	{
		public CMK()
		{
		}

		public CMK(int id, string name, List<string> compositions, string history, string descriptionOfWork, string achievements, string plans)
		{
			this.id = id;
			this.name = name;
			this.compositions = compositions;
			this.history = history;
			this.descriptionOfWork = descriptionOfWork;
			this.achievements = achievements;
			this.plans = plans;
		}

		public CMK(string name, List<string> compositions, string history, string descriptionOfWork, string achievements, string plans)
		{
			this.name = name;
			this.compositions = compositions;
			this.history = history;
			this.descriptionOfWork = descriptionOfWork;
			this.achievements = achievements;
			this.plans = plans;
		}

		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int id { get; set; }
		public string name { get; set; } = "ЦМК";
		public string url { get; set; } = "CMK";
		public List<string> compositions { get; set; } = [];
		public string history { get; set; } = "История ЦМК";
		public string descriptionOfWork { get; set; } = "Описание работы ЦМК";
		public string achievements { get; set; } = "Достижения ЦМК";
		public string plans { get; set; } = "Планы и задачи, которые решает ЦМК";
	}
}
