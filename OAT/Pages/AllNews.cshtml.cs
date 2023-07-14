using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.function;

namespace OAT.Pages
{
    public class AllNewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<NewsController.NewsData> news { get; set; }
        public int pages { get; set; }

        public AllNewsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            try
            {
                pages = NewsController.pages.Count();

                if (string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                    news = NewsController.pages[0];
                else
                    news = NewsController.pages[Convert.ToInt32(HttpContext.Request.Query["id"]) - 1];
            }
            catch { }

        }
    }
}