using OAT.Entities;

namespace OAT.Controllers.Bitrix.Controllers
{
	public class UrlsContoller
	{
		private static List<IPage> pages = [];
		public static void Add(IPage page) => pages.Add(page);

		public static string? Redirect(string path)
		{
			var cPage = pages.Where(p => $"/{p.current}" == path);
			if (cPage.Count() != 0)
				return cPage.FirstOrDefault()?.current;

			var page = pages.Where(p => $"/{p.map}" == path || $"/{p.map}/" == path).FirstOrDefault();
			if (path.Contains("raspisanie"))
				page = new IPage("raspisanie/*", "timetable/Classes");
			return page == null ? null : page.current;
		}

		public static Task init()
		{
			var urls = new List<IPage>()
		{
			/*
			 * 1) Если страница готова, то в первом аргументе прописываем  относительную ссылку битрикса, 
			 * а во втором аргументе - новую относительную ссылку в ASP
			 * 
			 * 2) Если страница не готова, то в первом аргументе прописываем новую относительную ссылку,
			 * а во втором аргументе - относительную ссылку битрикса со "/" в конце!
			 * Во всех предыдущих случаях на других страницах мы указываем новую относительную ссылку
			 * 
			 * 3) Если файл страницы вообще отсутствует, то на других страницах, ссылающихся на данную, 
			 * мы указываем относительную ссылку на битрикс. В файле UrlsController ничего не указываем.
			 * ШАБЛОН: new IPage("ссылка, которую мы заменяем", "ссылка, на которую мы заменяем")
			 */ 

			// main links
			new("novosti", "AllNews"),
			new("admin/panel", "admin/news"),
			new("abitur", "Applicant"),
			new("students", "Students"),
			// organization information
			new("sveden/ovz", "organizationInformation/AccessibleEnvironment"),
			new("sveden/common", "organizationInformation/CommonIntelligence"),
			new("sveden", "organizationInformation/CommonIntelligence"),
			new("sveden/corup/index.php", "organizationInformation/Corruption"),
			new("sveden/document", "organizationInformation/Documents"),
			new("sveden/education", "organizationInformation/Education"),
			new("parents/organizatsiya-vospitatelnogo-protsessa-v-kolledzhe", "organizationInformation/EducationalProcess"),
			new("sveden/obrStandart.php", "organizationInformation/EducationalStandards"),
			new("sveden/budget", "organizationInformation/FinancialEconomicActivity"),
			new("sveden/inter", "organizationInformation/InternationalCooperation"),
			new("sveden/inter", "organizationInformation/InternationalCooperation"),
			new("sveden/vacant", "organizationInformation/Jobs"),
			new("sveden/biblioteka", "organizationInformation/Library"),
			new("sveden/objects", "organizationInformation/Logistics"),
			new("sveden/paid_edu", "organizationInformation/PaidEducationalServices"),
			new("sveden/struct", "organizationInformation/Structure"),
			new("sveden/grants", "organizationInformation/StudentSupport"),
			new("sveden/terror/index.php", "organizationInformation/Terrorism"),
			new("sveden/vacancy", "organizationInformation/Vacancies"),

			// applicants
			new("abitur/priemnaya-komissiya", "applicant/SelectionCommittee"),
			new("abitur/spetsialnosti", "applicant/SpecialtiesAndProfessions"),
			new("abitur/podgotovitelnye-kursy", "applicant/TrainingCourses"),
			new("abitur/individualnoe-obuchenie", "applicant/IndividualTraining"),
			new("abitur/pravila-priema", "applicant/AdmissionRules"),
			new("abitur/povyshenie-kvalifikatsii", "applicant/QualificationIncrease"),
			new("abitur/sertifikatsiya-d-link-cisco", "applicant/DLinkCisco"),
			new("abitur/spetsialnosti-ochno-zaochnogo-vechernego-otdeleniya/", "applicant/SpecialtiesPartTimeEvening"),
			new("abitur/prof.php", "applicant/CareerGuidanceEvents"),

			// students
			new("students/distant", "students/DistanceFullTimeLearning"),
			new("students/distantsionnoe-obuchenie", "students/DistanceLearning"),
			new("students/sport/index.php", "students/SportClub"),
			new("sveden/Employment/Employment.php", "students/AlumniEmploymentCenterOmaviat"),
			new("sveden/Employment/center.php", "students/AlumniEmploymentCenterOmsk"),
			new("parents/pravila-vnutrennego-rasporyadka-dnya-studentov", "students/DailyRoutineRules"),

			// parents
			new("parents/organizatsiya-uchebnogo-protsessa-v-kolledzhe", "parents/EducationalProcessOrganization"),

			// feedback
			new("contacts/kak-dobratsya", "feedback/Location"),
			new("contacts/", "feedback/Contacts"),
			new("contacts/vopros-otvet/", "feedback/QuestionAnswer"),
			new("pay/", "feedback/Pay"),
			// PROJECTS AND EVENTS
			// professionalitet

			// limitation
			//new IPage("organizationInformation/ManagementPedagogicalStaff", "sveden/employees/"),
			new("feedback/Map", "skhema-kolledzha/"),
		   // new IPage("parents/AcademicProgress", "parents/uspevaemost/"),
		};
			pages = urls;
			return Task.CompletedTask;
		}
	}
}