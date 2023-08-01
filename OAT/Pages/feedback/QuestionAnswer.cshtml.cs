using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace OAT.Pages.feedback
{
    [ValidateReCaptcha("questionanswer", ErrorMessage = "Вы не прошли капчу")]
    public class QuestionAnswerModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        public QuestionAnswerModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet() { }
        [BindProperty, Required(ErrorMessage = "Вы не заполнили ваше имя")] public string name { get; set; }
        [BindProperty, Required(ErrorMessage = "Вы не заполнили вашу почту")] public string email { get; set; }
        [BindProperty, Required(ErrorMessage = "Вы не заполнили ваш телефон")] public string telephone { get; set; }
        [BindProperty, Required(ErrorMessage = "Вы не заполнили ваш вопрос")] public string description { get; set; }
        [BindProperty, Required(ErrorMessage = "Вы не выбрали тему")] public string topic { get; set; }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
                return Page();
            var sendTo = string.Empty;
            switch (topic)
            {
                case "Директору":
                    sendTo = "";
                    break;
                case "Зам.Директора":
                    sendTo = "";
                    break;
                default:
                    sendTo = "";
                    break;
            }

            Utils.SendEmail(email, sendTo, $"" +
                $"Имя: {name}\n" +
                $"Почта для ответа: {email}\n" +
                $"Телефон: {telephone}\n" +
                $"Вопрос: {description}");

            return RedirectToPage();
        }
    }
}