using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Utilities;

namespace OAT.Pages.timetable
{
	[NoCache]
	public class PracticeModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public PracticeModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public List<FilePractice> b1 = new List<FilePractice>();
		public List<FilePractice> b2 = new List<FilePractice>();
		public List<FilePractice> b3 = new List<FilePractice>();
		public List<FilePractice> b4 = new List<FilePractice>();

		public void OnGet()
		{
			for (int i = 1; i <= 4; i++)
			{
				var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "practice", $"b{i}");
				var files = Directory.GetFiles(folder, "*.*", SearchOption.TopDirectoryOnly).ToList();
				GetList(i).AddRange(files.ConvertAll(e => new FilePractice(StringUtils.ConvertHexToString(Path.GetFileName(e).Replace(Path.GetExtension(e), "")),
					$"api/practice/b{i}/{StringUtils.ConvertStringToHex(Path.GetFileName(e))}/download")));

			}
		}

		private List<FilePractice> GetList(int i)
		{
			return i switch
			{
				1 => b1,
				2 => b2,
				3 => b3,
				4 => b4,
				_ => new List<FilePractice>()
			};
		}

		public class FilePractice
		{
			public FilePractice(string filename, string url)
			{
				Filename = filename;
				this.url = url;
			}

			public string Filename { get; set; }
			public string url { get; set; }
		}
	}
}