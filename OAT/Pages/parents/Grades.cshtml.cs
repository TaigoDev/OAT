using Microsoft.AspNetCore.Mvc.RazorPages;
using OAT.Readers;
using OAT.Utilities;

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

        public void OnGet()
        {
            var username = Utils.Base64Decode(HttpContext.GetCookie("student-username"));
            var password = Utils.Base64Decode(HttpContext.GetCookie("student-password"));
            if (!Ldap.Login(username, password, HttpContext.UserIP(), false))
            {
                HttpContext.Response.Redirect("AcademicProgress");
                return;
            }
            //FullName = Ldap.GetFullName(username) ?? "Пользователь не найден";
            //var groupName = Ldap.GetStudentGroup(username);
            var groupName = "АВ211";
            FullName = "Герман Егор Владимирович";
            month = "Июнь";
            if (groupName is not null)
                Disciplines = EvaluationsReader.Search(groupName, FullName, "Июнь").GetAwaiter().GetResult();
        }

    }
}