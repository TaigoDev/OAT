using OAT.Entities.Enums;
using OAT.Entities.Schedule;

namespace OMAVIAT.Entities.Schedule
{
	public class CorpusSchedule
	{
		public int building { get; set; }
		public List<Schedule> groups { get; set; } = [];
		public List<Schedule> teachers { get; set; } = [];
		public List<Schedule> cabinets { get; set; } = [];
		public List<Bell> bells { get; set; } = [];
	}
}
