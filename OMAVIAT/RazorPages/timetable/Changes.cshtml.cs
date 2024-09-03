using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using OMAVIAT.Entities.Database;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Entities.Schedule.Database;
using OMAVIAT.Schedule.Entities.Enums;

namespace OMAVIAT.Pages
{
	[NoCache]
	public class ChangesModel : PageModel
	{
		private readonly ILogger<ChangesModel> _logger;

		public ChangesModel(ILogger<ChangesModel> logger)
		{
			_logger = logger;
		}

		public DateOnly? sheet { get; set; }
		public Corpus? corpus { get; set; }
		public string captchaToken { get; set; }
		public DaysChangesTable? daysChanges { get; set; }
		public void OnGet(string? corpus, string? sheet)
		{

			if (corpus is null)
				return;

			if (!Enum.TryParse<Corpus>(corpus, true, out var result))
				return;
			using var db = new DatabaseContext();

			this.corpus = result;
			if (DateOnly.TryParseExact(sheet, "dd.MM.yyyy", out var date))
			{
				this.sheet = date;
				daysChanges = db.daysChanges.Include(e => e.bells).
					FirstOrDefault(e => e.corpus == result && e.date == date);
				return;
			}

			var search = db.daysChanges.Include(e => e.bells).
				Where(e => e.corpus == result).ToList().MaxBy(e => e.date);
			if (search is null) return;
			this.sheet = search.date;
			daysChanges = search;
		}
	}

}