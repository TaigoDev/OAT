using Ganss.Excel;
using NPOI.SS.Formula.Functions;

namespace OAT.Modules.Workers
{
	public class WorkersReader
	{
		public static List<List<Worker>> administration = new List<List<Worker>>();
		public static IEnumerable<IEnumerable<Worker>> Workers = new List<List<Worker>>();
		public static List<Worker> AllWorkers = new List<Worker>();


		public static async Task init()
		{
			using var stream = File.Open(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "workers", "workers.xlsx"), FileMode.Open, FileAccess.Read);

			var excel = new ExcelMapper()
			{
				HeaderRow = true,
				HeaderRowNumber = 7,
				MinRowNumber = 8,
				CreateMissingHeaders = true,
			};

			var reader = await excel.FetchAsync<Worker>(stream);
			var workers = reader.ToList();
			var ignoredPost = new List<string>()
			{
				"Уборщик служебных помещений",
				"Рабочий по комплексному обслуживанию и ремонту зданий",
				"Слесарь-сантехник",
				"Сторож (вахтёр)",
				"Дежурный по общежитию",
				"Электромонтер по ремонту и обслуживанию электрооборудования",
				"Инженер-электроник (электроник)",
				"Кассир",
				"Плотник",
				"Воспитатель",
				"Гардеробщик",
				"Дворник",
				"Инженер",
				"Кладовщик",
				"Маляр",
				"Механик",
				"Плотник",
				"Слесарь-ремонтник",
				"Табельщик",
				"Фельдшер",
				"Водитель автомобиля",
				"Лаборант",
				"Старший лаборант",
				"Старший мастер",
				"Уборщик производственных помещений"
			};

			workers.RemoveAll(e => ignoredPost.Contains(e.Post));
			AllWorkers = workers.ToList();
			AdministrationByPost(ref workers, e => e is "Директор");
			AdministrationByPost(ref workers, e => e.Contains("Заместитель директора"));
			AdministrationByPost(ref workers, e => e is "Главный бухгалтер");

			Workers = workers.PagesSplit(14);
		}

		private static void AdministrationByPost(ref List<Worker> workers, Func<string, bool> predicate)
		{
			administration.Add(workers.Where(e => predicate.Invoke(e.Post)).ToList());
			workers.RemoveAll(e => predicate.Invoke(e.Post));
		}

		public class Worker
		{
			[Column("Сотрудник")]
			public string FullName { get; set; }

			[Column("Должность")]
			public string Post { get; set; }

			[Column("Ученая степень.Наименование")]
			public string AcademicDegree { get; set; }


			[Column("Образование 1 вид образования")]
			public string Education1_Type { get; set; }

			[Column("Образование 1 учебное заведение")]
			public string Education1_EducationalInstitution { get; set; }

			[Column("Образование 1 дата выдачи")]
			public string Education1_issued { get; set; }

			[Column("Образование 1 специальность")]
			public string Education1_specialization { get; set; }



			[Column("Образование 2 вид образования")]
			public string Education2_Type { get; set; }

			[Column("Образование 2 учебное заведение")]
			public string Education2_EducationalInstitution { get; set; }

			[Column("Образование 2 дата выдачи")]
			public string Education2_issued { get; set; }

			[Column("Образование 2 специальность")]
			public string Education2_specialization { get; set; }



			[Column("Повышение квалификации")]
			public string Qualification { get; set; }

			[Column("Переподготовка")]
			public string ProfessionalDevelopment { get; set; }


			[Column("Стаж работы на предприятии лет")]
			public string ExperienceInOAT { get; set; }

			[Column("Общий стаж лет")]
			public string AllExperience { get; set; }

			[Column("Педагогический стаж лет")]
			public string TeacherExperience { get; set; }
		}

	}
}
