
public class UrlsContoller
{
    private static List<IPage> pages = new List<IPage>();
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
            new IPage("novosti", "AllNews"),
            new IPage("abitur", "Applicant"),
            new IPage("students", "Students"),
            // organization information
            new IPage("sveden/ovz", "organizationInformation/AccessibleEnvironment"),
            new IPage("sveden/common", "organizationInformation/CommonIntelligence"),
            new IPage("sveden", "organizationInformation/CommonIntelligence"),
            new IPage("sveden/corup/index.php", "organizationInformation/Corruption"),
            new IPage("sveden/document", "organizationInformation/Documents"),
            new IPage("sveden/education", "organizationInformation/Education"),
            new IPage("parents/organizatsiya-vospitatelnogo-protsessa-v-kolledzhe", "organizationInformation/EducationalProcess"),
            new IPage("sveden/obrStandart.php", "organizationInformation/EducationalStandards"),
            new IPage("sveden/budget", "organizationInformation/FinancialEconomicActivity"),
            new IPage("sveden/inter", "organizationInformation/InternationalCooperation"),
            new IPage("sveden/inter", "organizationInformation/InternationalCooperation"),
            new IPage("sveden/vacant", "organizationInformation/Jobs"),
            new IPage("sveden/biblioteka", "organizationInformation/Library"),
            new IPage("sveden/objects", "organizationInformation/Logistics"),
            new IPage("sveden/paid_edu", "organizationInformation/PaidEducationalServices"),
            new IPage("sveden/struct", "organizationInformation/Structure"),
            new IPage("sveden/grants", "organizationInformation/StudentSupport"),
            new IPage("sveden/terror/index.php", "organizationInformation/Terrorism"),
            new IPage("sveden/vacancy", "organizationInformation/Vacancies"),

            // applicants
            new IPage("abitur/priemnaya-komissiya", "applicant/SelectionCommittee"),
            new IPage("abitur/spetsialnosti", "applicant/SpecialtiesAndProfessions"),
            new IPage("abitur/podgotovitelnye-kursy", "applicant/TrainingCourses"),
            new IPage("abitur/individualnoe-obuchenie", "applicant/IndividualTraining"),
            new IPage("abitur/pravila-priema", "applicant/AdmissionRules"),
            new IPage("abitur/povyshenie-kvalifikatsii", "applicant/QualificationIncrease"),
            new IPage("abitur/sertifikatsiya-d-link-cisco", "applicant/DLinkCisco"),
            new IPage("abitur/spetsialnosti-ochno-zaochnogo-vechernego-otdeleniya/", "applicant/SpecialtiesPartTimeEvening"),
            new IPage("abitur/prof.php", "applicant/CareerGuidanceEvents"),

            // students
            new IPage("students/distant", "students/DistanceFullTimeLearning"),
            new IPage("students/distantsionnoe-obuchenie", "students/DistanceLearning"),
            new IPage("students/sport/index.php", "students/SportClub"),
            new IPage("sveden/Employment/Employment.php", "students/AlumniEmploymentCenterOmaviat"),
            new IPage("sveden/Employment/center.php", "students/AlumniEmploymentCenterOmsk"),
            new IPage("parents/pravila-vnutrennego-rasporyadka-dnya-studentov", "students/DailyRoutineRules"),

            // parents
            new IPage("parents/organizatsiya-uchebnogo-protsessa-v-kolledzhe", "parents/EducationalProcessOrganization"),

            // feedback
            new IPage("contacts/kak-dobratsya", "feedback/Location"),
            new IPage("contacts/", "feedback/Contacts"),
            new IPage("contacts/vopros-otvet/", "feedback/QuestionAnswer"),
            new IPage("pay/", "feedback/Pay"),
            // PROJECTS AND EVENTS
            // professionalitet

            // limitation
            new IPage("organizationInformation/ManagementPedagogicalStaff", "sveden/employees/"),
            new IPage("feedback/Map", "skhema-kolledzha/"),
            new IPage("parents/AcademicProgress", "parents/uspevaemost/"),
        };
        pages = urls;
        return Task.CompletedTask;
    }
}


