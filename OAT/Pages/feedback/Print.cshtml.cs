using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Net.Codecrete.QrCodeGenerator;
using OAT.Controllers.Payments.Readers;
using OAT.Entities;
using OAT.Utilities;
using System.Text;

namespace OAT.Pages.feedback
{
	public class PrintModel : PageModel
	{
		private readonly ILogger<IndexModel> _logger;

		public PrintModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public string M_Purpose;
		public string M_FullName;
		public int M_summa;
		public string qrPathIzv;
		public string qrPathQvit;

		public void OnGet([FromQuery] int purpose, [FromQuery] int Voluntary_Purpose, [FromQuery] string documentId, [FromQuery] string documentDate,
			[FromQuery] string studentFullName, [FromQuery] string group, [FromQuery] string FullName, [FromQuery] int summa, [FromQuery] string email,
			[FromQuery] string phone)
		{
			if (purpose == 4)
			{
				M_Purpose = $"Добровольное пожертвование{GetVoluntaryPurpose(Voluntary_Purpose)}";
				M_FullName = FullName;
				M_summa = summa;
				var qrData = $"ST00012|Name=Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)|PersonalAcc=03224643520000005201|BankName=Отделение Омск Банка России//УФК по Омской области г. Омск|BIC=015209001|CorrespAcc=0|PayeeINN=5504000055|KPP=550401001|" +
					$"Purpose={M_Purpose}|Sum={summa * 100}|CBC=01000000000000000130|OKTMO=52701000";
				var qr = QrCode.EncodeText(qrData, QrCode.Ecc.Medium);

				var pathIzv = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay",
					$"{StringUtils.SHA226($"{new Random().Next(1000)}-{documentId}-{group}")}.svg");
				var pathQvit = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay",
					$"{StringUtils.SHA226($"{new Random().Next(1000)}-{documentId}-{group}")}.svg");

				System.IO.File.WriteAllText(pathIzv, qr.ToSvgString(4), Encoding.UTF8);
				System.IO.File.WriteAllText(pathQvit, qr.ToSvgString(4), Encoding.UTF8);

				qrPathIzv = $"pay/download/{new FileInfo(pathIzv).Name}";
				qrPathQvit = $"pay/download/{new FileInfo(pathQvit).Name}";
			}
			else
			{
				var contract = new Contract();
				if (!ContractReader.GetContract(e =>
				e.documentId.ToSearchView() == documentId.ToSearchView() &&
				e.documentDate.ToSearchView() == documentDate.ToSearchView() &&
				e.studentFullName.ToSearchView() == studentFullName.ToSearchView() &&
				e.Group.ToSearchView() == group.ToSearchView() &&
				e.FullName.ToSearchView() == FullName.ToSearchView(), out contract))
					return;


				M_Purpose = $"{GetPurpose(purpose)} по дог.№ {contract.documentId.Replace(" ", "")}, {contract.studentFullName} гр.№ {contract.Group}";
				M_FullName = contract.FullName;
				M_summa = summa;

				var qrData = $"ST00012|Name=Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)|PersonalAcc=03224643520000005201|BankName=Отделение Омск Банка России//УФК по Омской области г. Омск|BIC=015209001|CorrespAcc=0|PayeeINN=5504000055|KPP=550401001|" +
					$"Purpose={M_Purpose}|Contract={contract.documentId.Replace(" ", "")}|ChildFio={contract.studentFullName}|Sum={summa}|" +
					$"CBC=01000000000000000130|OKTMO=52701000|CATEGORY=1";

				var qr = QrCode.EncodeText(qrData, QrCode.Ecc.Medium);

				var pathIzv = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay",
					$"{StringUtils.SHA226($"{new Random().Next(1000)}-{documentId}-{group}")}.svg");
				var pathQvit = Path.Combine(Directory.GetCurrentDirectory(), "Resources", "pay",
					$"{StringUtils.SHA226($"{new Random().Next(1000)}-{documentId}-{group}")}.svg");

				System.IO.File.WriteAllText(pathIzv, qr.ToSvgString(4), Encoding.UTF8);
				System.IO.File.WriteAllText(pathQvit, qr.ToSvgString(4), Encoding.UTF8);

				qrPathIzv = $"pay/download/{new FileInfo(pathIzv).Name}";
				qrPathQvit = $"pay/download/{new FileInfo(pathQvit).Name}";
			}

		}


		private string GetVoluntaryPurpose(int id)
		{
			switch (id)
			{
				case 0:
					return " на приобретение спортивного инвентаря";
				case 1:
					return " на приобретение хозяйственного инвентаря";
				case 2:
					return " на приобретение строительных материалов";
				case 3:
					return " на приобретение учебного оборудования и расходных материалов";
				case 4:
					return " на выполнение ремонтных работ";
				case 5:
					return " на приобретение оргтехники и расходных материалов";
				default:
					return "";
			}
		}

		private string GetPurpose(int id)
		{
			switch (id)
			{
				case 0:
					return "Платное обучение";
				case 1:
					return "Дистанционное обучение";
				case 2:
					return "Дополнительные образовательные услуги";
				case 3:
					return "Оказание платных услуг";
				case 4:
					return "Добровольное пожертвование";
				case 5:
					return "Подготовительные курсы";
				case 6:
					return "Профессиональная переподготовка";
				default:
					return "Оплата по договору";

			}
		}
	}
}