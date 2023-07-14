using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.function;

namespace OAT.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        public List<NewsController.NewsData> News = new List<NewsController.NewsData>();
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            for (int i = 0; i < (NewsController.News.Count() >= 8 ? 8 : NewsController.News.Count()); i++)
                News.Add(NewsController.News[i]);
        }
    }
}