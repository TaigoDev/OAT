using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Utilities;

namespace OMAVIAT.Pages.timetable;

[NoCache]
public class SessionModel : PageModel
{
	private readonly ILogger<SessionModel> _logger;

	public List<FileSession> b1 = new();
	public List<FileSession> b2 = new();
	public List<FileSession> b3 = new();
	public List<FileSession> b4 = new();

	public SessionModel(ILogger<SessionModel> logger)
	{
		_logger = logger;
	}

	public void OnGet()
	{
		for (var i = 1; i <= 4; i++)
		{
			var folder = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "sessions", $"b{i}");
			var files = Directory.GetFiles(folder, "*.xlsx", SearchOption.TopDirectoryOnly).ToList();
			GetList(i).AddRange(files.ConvertAll(e => new FileSession(
				StringUtils.ConvertHexToString(Path.GetFileNameWithoutExtension(e)),
				$"api/sessions/b{i}/{Path.GetFileNameWithoutExtension(e)}/download")));
		}
	}

	private List<FileSession> GetList(int i)
	{
		return i switch
		{
			1 => b1,
			2 => b2,
			3 => b3,
			4 => b4,
			_ => new List<FileSession>()
		};
	}

	public class FileSession
	{
		public FileSession(string filename, string url)
		{
			Filename = filename;
			this.url = url;
		}

		public string Filename { get; set; }
		public string url { get; set; }
	}
}