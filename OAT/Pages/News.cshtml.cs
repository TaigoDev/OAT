using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class NewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public NewsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }
        public News news { get; set; }
        public List<string> text = new List<string>();

        public void OnGet()
        {
            if (HttpContext.Request.Query["id"].ToString() == null)
                Redirect("~");
            var id = Convert.ToInt32(HttpContext.Request.Query["id"]);
            if (id > NewsController.News.Count())
                Redirect("~");

            news = NewsController.News.Where(n => n.id == id).First();
            text.AddRange(news.text.Split("\n"));
        }
    }
}