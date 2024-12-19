using Microsoft.AspNetCore.Mvc.RazorPages;
using OMAVIAT.Entities.Database;

namespace OMAVIAT.Pages {
	public class IndexModel : PageModel {
		private readonly ILogger<IndexModel> _logger;
		public List<News> News = new();
		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			for (int i = 0; i < (NewsReader.news.Count() >= 9 ? 9 : NewsReader.news.Count()); i++)
				News.Add(NewsReader.news[i]);
		}
	}
}
