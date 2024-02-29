using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.UtilsHelper.ReCaptcha;
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
			var sendTo = topic switch
			{
				"Профессионалитет" or "Производство" => "savin@oat.ru",
				"Воспитательная работа" => "kameneva@oat.ru",
				"Учебная деятельность" => "trockaya@oat.ru",
				"Профориентация" or "Трудоустройство" or "Приемная комиссия" or "Курсовая подготовка" => "savin@oat.ru",
				"Главная бухгалтерия" or "Бухгалтерия" or "Экономика" => "dolgusheva_ny@oat.ru",
				"Директор школы" => "subbotin.sa@oat.ru",
				"Инженерный лицей" => "elo@oat.ru",
				_ => "post@oat.ru"
			};

			Utils.SendEmail(email, sendTo, $"" +
				$"Имя: {name}\n" +
				$"Почта для ответа: {email}\n" +
				$"Телефон: {telephone}\n" +
				$"Тема: {topic}\n" +
				$"Вопрос: {description}");

			return RedirectToPage();
		}



	}
}
