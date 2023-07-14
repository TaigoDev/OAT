using Microsoft.AspNetCore.Mvc.RazorPages;

namespace OAT.Pages.feedback
{
    public class QuestionAnswerModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public QuestionAnswerModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {

        }
    }
}