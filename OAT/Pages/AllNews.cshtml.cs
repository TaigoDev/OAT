using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class AllNewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<News> news = new List<News>(); //fix error load news
        public int pages { get; set; }

        public AllNewsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            Console.WriteLine("Join");
            try
            {
                Console.WriteLine(NewsController.pages.Count());

                pages = NewsController.pages.Count();
                Console.WriteLine(pages);
                if (string.IsNullOrEmpty(HttpContext.Request.Query["id"]))
                    news = NewsController.pages[0];
                else
                    news = NewsController.pages[Convert.ToInt32(HttpContext.Request.Query["id"]) - 1];
                Console.WriteLine(news.Count());

            }
            catch (Exception ex) {
                Logger.Error(ex.ToString());
            }

        }
    }
}