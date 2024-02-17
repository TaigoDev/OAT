using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;
using OAT.Utilities;
using OAT.UtilsHelper;
using System.Globalization;

namespace OAT.Pages.parents
{
	[NoCache]
	public class GradesModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public GradesModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public string FullName { get; set; }

		public Student? Disciplines { get; set; }

		public string month { get; set; }

		public void OnGet(string? month)
		{
			var cookieUsername = HttpContext.GetCookie("student-username");
			var cookiePassword = HttpContext.GetCookie("student-password");

			if (cookieUsername is null || cookiePassword is null)
			{
				HttpContext.Response.Redirect("AcademicProgress");
				return;
			}

			var username = StringUtils.Base64Decode(cookieUsername);
			var password = StringUtils.Base64Decode(cookiePassword);
			if (!Ldap.Login(username, password, HttpContext.UserIP(), false))
			{
				HttpContext.Response.Redirect("AcademicProgress");
				return;
			}
			FullName = Ldap.GetFullName(username) ?? "Пользователь не найден";
			var groupName = Ldap.GetStudentGroup(username);

			this.month = month ?? DateTime.Now.ToString("MMMM", CultureInfo.GetCultureInfo("ru-ru"));
			var blackList = new string[] { "август", "июль" };
			if (groupName is not null)
				Disciplines = !blackList.Contains(month) ? EvaluationsReader.Search(groupName, FullName, this.month).GetAwaiter().GetResult() : null;
		}

	}
}