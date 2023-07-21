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
            for (int i = 0; i < (NewsController.News.Count() >= 9 ? 9 : NewsController.News.Count()); i++)
                News.Add(NewsController.News[i]);
        }
    }
}