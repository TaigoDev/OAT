using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<News> News = new List<News>();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            for (int i = 0; i < (NewsReader.News.Count() >= 9 ? 9 : NewsReader.News.Count()); i++)
                News.Add(NewsReader.News[i]);
        }
    }
}