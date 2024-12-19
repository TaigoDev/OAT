using Ganss.Excel;

namespace OMAVIAT.Entities.Models {
	public class Worker {
		[Column("Сотрудник")]
		public string? FullName { get; set; }

		[Column("Должность")]
		public string? Post { get; set; }

		[Column("Ученая степень.Наименование")]
		public string? AcademicDegree { get; set; }

		[Column("Дисциплины")]
		public string? Subjects { get; set; }

		[Column("Категория")]
		public string? Сategory { get; set; }

		[Column("Образование 1 вид образования")]
		public string? Education1_Type { get; set; }

		[Column("Образование 1 учебное заведение")]
		public string? Education1_EducationalInstitution { get; set; }

		[Column("Образование 1 дата выдачи")]
		public string? Education1_issued { get; set; }

		[Column("Образование 1 специальность")]
		public string? Education1_specialization { get; set; }



		[Column("Образование 2 вид образования")]
		public string? Education2_Type { get; set; }

		[Column("Образование 2 учебное заведение")]
		public string? Education2_EducationalInstitution { get; set; }

		[Column("Образование 2 дата выдачи")]
		public string? Education2_issued { get; set; }

		[Column("Образование 2 специальность")]
		public string? Education2_specialization { get; set; }



		[Column("Повышение квалификации")]
		public string? Qualification { get; set; }

		[Column("Переподготовка")]
		public string? ProfessionalDevelopment { get; set; }


		[Column("Стаж работы на предприятии лет")]
		public string? ExperienceInOAT { get; set; }

		[Column("Общий стаж лет")]
		public string? AllExperience { get; set; }

		[Column("Педагогический стаж лет")]
		public string? TeacherExperience { get; set; }
	}
}
