using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages
{
    public class AllNewsModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;


        public AllNewsModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }


    }
}