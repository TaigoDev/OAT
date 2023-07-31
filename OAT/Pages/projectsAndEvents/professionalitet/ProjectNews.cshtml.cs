using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class ProjectNewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public List<News> news = new List<News>(); //fix error load news
        public int pages { get; set; }

        public ProjectNewsModel(ILogger<IndexModel> logger)
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
            catch (Exception ex)
            {
                Logger.Error(ex.ToString());
            }

        }
    }
}