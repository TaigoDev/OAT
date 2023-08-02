
using Microsoft.AspNetCore.Mvc;
using Net.Codecrete.QrCodeGenerator;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using Svg;
using System.Drawing.Imaging;
using System.Net.NetworkInformation;
using System.Text;

namespace OAT.Controllers
{
    public class OplataController : Controller
    {

        [HttpPost("api/oplata/receipt")]
        public IActionResult CreateReceipt(string? documentId, string? documentDate, string? studentFullName, string FullName, string group, int summa, string purpose, string email, string phone)
        {

            var documentPurpose = $"{purpose} по дог.№ {documentId}, {studentFullName}, гр.№ {group}";
            var qrData = $"ST00012|Name=Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)|PersonalAcc=03224643520000005201|BankName=Отделение Омск Банка России//УФК по Омской области г. Омск|BIC=015209001|CorrespAcc=0|PayeeINN=5504000055|KPP=550401001|" +
                $"Purpose={documentPurpose}|Contract={documentId}|ChildFio={studentFullName}|Sum={Convert.ToInt32(summa) * 100}" +
                $"|CBC=01000000000000000130|OKTMO=52701000|CATEGORY=1";

            var qr = QrCode.EncodeText(qrData, QrCode.Ecc.Medium);
            var path = Path.Combine(Directory.GetCurrentDirectory(), "pay",
                $"{Utils.sha256_hash($"{new Random().Next(1000)}-{documentId}-{group}")}.svg");

            System.IO.File.WriteAllText(path, qr.ToSvgString(4), Encoding.UTF8);
            //var pathPng = SaveToPng(path);
			GeneratePdfFile(path, purpose, summa.ToString(), FullName);
			//Utils.FileDelete(pathPng);
			Utils.FileDelete(path);
			var filename = new FileInfo(path.Replace(".svg", ".pdf")).Name;
			return Ok($"/pay/download/{filename}");
        }

		[HttpGet("pay/download/{filename}")]
		public IActionResult DownloadFile(string filename)
		{
			var path = Path.Combine(Directory.GetCurrentDirectory(), "pay", filename);
			var bytes = System.IO.File.ReadAllBytes(path);
			System.IO.File.Delete(path);
			return File(bytes, "application/pdf");

		}

		protected void GeneratePdfFile(string png, string purpose, string summa, string FullName)
        {
			var document = Document.Create(container =>
			{
				container.Page(page =>
				{
					page.Header().AlignCenter().PaddingTop(8)
						.Text("Внимание! Документ создан автоматически, проверьте данные перед оплатой").
						FontSize(12).FontColor(Colors.Red.Darken4).Bold().FontFamily("Arial");

					page.Content()
						.MinimalBox().AlignCenter().Border(0).Padding(8).Table(table =>
						{

							table.ColumnsDefinition(columns =>
							{
								columns.ConstantColumn(200);
								columns.ConstantColumn(370);

								columns.RelativeColumn(2);
								columns.RelativeColumn(2);
							});
							table.Cell().Border(1).AlignCenter().AddBoardQr("ИЗВЕЩЕНИЕ", png);
							table.Cell().Border(1).Padding(6).Text(e =>
							{
								e.Line("Омский авиационный колледж имени Н.Е. Жуковского").FontFamily("Arial").FontSize(12);
								e.Line("\nПолучатель: Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)").FontFamily("Arial").FontSize(12);
								e.Span("ИНН: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("5504000055 ").FontFamily("Arial").FontSize(12);
								e.Span("КПП: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("550401001 ").FontFamily("Arial").FontSize(12);
								e.Span("БИК: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("015209001\n").FontFamily("Arial").FontSize(12);
								e.Span("КБК: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("01000000000000000130").FontFamily("Arial").FontSize(12);
								e.Span("ОКТМО: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("52701000\n").FontFamily("Arial").FontSize(12);
								e.Element().RCAndKCTable();
								e.Line("в Отделение Омск Банка России//УФК по Омской области г. Омск Номер лицевого счёта: 010220608\n").FontFamily("Arial").FontSize(12);
								e.Line("Назначение платежа:").FontFamily("Arial").FontSize(12).Bold();
								e.Line($"{purpose}\n").FontFamily("Arial").FontSize(12);
								e.Span("Сумма: ").FontFamily("Arial").FontSize(12);
								e.Span(summa).Bold().FontFamily("Arial").FontSize(12);
								e.Span(" руб.\n").FontFamily("Arial").FontSize(12);
								e.Span("ФИО (плательщик): ").FontFamily("Arial").FontSize(12);
								e.Span(FullName).Underline().FontFamily("Arial").FontSize(12);
								e.Line("\n\nC условиями приема указанной в платежном документе суммы, в т.ч. суммой оплаты за услуги банка ознакомлен и согласен").FontFamily("Arial").FontSize(12);
								e.Line("\nПодпись: _____________ дата \"____\" ___________ 20___г.").FontFamily("Arial").FontSize(12);
							});
							table.Cell().Row(2).Border(1).AlignCenter().AddBoardQr("КВИТАНЦИЯ", "D:\\GitProjects\\TaigoDev\\OAT\\OAT\\pay\\0effb13158df9385f205dd02aee3402a889815065e33b62bedf14186f7895f3a.png");
							table.Cell().Border(1).Padding(6).Text(e =>
							{
								e.Line("Омский авиационный колледж имени Н.Е. Жуковского").FontFamily("Arial").FontSize(12);
								e.Line("\nПолучатель: Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)").FontFamily("Arial").FontSize(12);
								e.Span("ИНН: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("5504000055 ").FontFamily("Arial").FontSize(12);
								e.Span("КПП: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("550401001 ").FontFamily("Arial").FontSize(12);
								e.Span("БИК: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("015209001\n").FontFamily("Arial").FontSize(12);
								e.Span("КБК: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("01000000000000000130").FontFamily("Arial").FontSize(12);
								e.Span("ОКТМО: ").Bold().FontFamily("Arial").FontSize(12);
								e.Span("52701000\n").FontFamily("Arial").FontSize(12);
								e.Element().RCAndKCTable();
								e.Line("в Отделение Омск Банка России//УФК по Омской области г. Омск Номер лицевого счёта: 010220608\n").FontFamily("Arial").FontSize(12);
								e.Line("Назначение платежа:").FontFamily("Arial").FontSize(12).Bold();
								e.Line($"{purpose}\n").FontFamily("Arial").FontSize(12);
								e.Span("Сумма: ").FontFamily("Arial").FontSize(12);
								e.Span(summa).Bold().FontFamily("Arial").FontSize(12);
								e.Span(" руб.\n").FontFamily("Arial").FontSize(12);
								e.Span("ФИО (плательщик): ").FontFamily("Arial").FontSize(12);
								e.Span(FullName).Underline().FontFamily("Arial").FontSize(12);
								e.Line("\n\nC условиями приема указанной в платежном документе суммы, в т.ч. суммой оплаты за услуги банка ознакомлен и согласен").FontFamily("Arial").FontSize(12);
								e.Line("\nПодпись: _____________ дата \"____\" ___________ 20___г.").FontFamily("Arial").FontSize(12);
							});
						});

					page.Footer()
						.AlignCenter().PaddingBottom(20)
						.Text(x =>
						{
							x.Span($"© {DateTime.UtcNow.Year} ОМСКИЙ АВИАЦИОННЫЙ КОЛЛЕДЖ").FontFamily("Arial").Bold().FontSize(10);
						});
				});
			});
			document.GeneratePdf(png.Replace(".svg", ".pdf"));
			
		}


		public static string SaveToPng(string path)
		{
			var svgDocument = SvgDocument.Open(path);
			var bitmap = svgDocument.Draw();
			bitmap.Save(path.Replace(".svg", ".png"), ImageFormat.Png);
			return path.Replace(".svg", ".png");
		}



	}
	static class SimpleExtension
	{
		public static void RCAndKCTable(this IContainer container)
		{
			container.Table(table =>
			{
				table.ColumnsDefinition(columns =>
				{
					columns.ConstantColumn(40);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);
					columns.ConstantColumn(15);

					columns.RelativeColumn(5);
					columns.RelativeColumn(5);
				});
				var numbers = "03224643520000005201";
				table.Cell().Border(1).AlignCenter().Text("Р/C").FontFamily("Arial");
				foreach (var str in numbers)
					table.Cell().Border(1).AlignCenter().Text(str.ToString()).FontFamily("Arial");
				table.Cell().Row(2).Border(1).AlignCenter().Text("К/C").FontFamily("Arial");
				numbers = "40102810245370000044";
				foreach (var str in numbers)
					table.Cell().Border(1).AlignCenter().Text(str.ToString()).FontFamily("Arial");
			});
		}

		public static void AddBoardQr(this IContainer container, string name, string file)
		{
			container.Table(table =>
			{
				table.ColumnsDefinition(columns =>
				{
					columns.ConstantColumn(200);
					columns.ConstantColumn(200);

					columns.RelativeColumn(2);
					columns.RelativeColumn(2);
				});
				table.Cell().Row(1).Padding(10).AlignCenter().Image(Placeholders.Image(250, 250));
				table.Cell().Row(2).BorderTop(1).PaddingLeft(2).Padding(20).PaddingBottom(100).AlignCenter().Text(name).FontFamily("Arial");
			});
		}

	}
}
