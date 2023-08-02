using Microsoft.AspNetCore.Mvc;
using Net.Codecrete.QrCodeGenerator;
using System.Text;

namespace OAT.Controllers
{
    public class OplataController : Controller
    {
        //[HttpPost("api/oplata/receipt/}")]
        public static void CreateReceipt(string? documentId, string? documentDate, string? studentFullName, 
            string FullName, string group, int summa, string purpose)
        {
            if(purpose != "Добровольное пожертвование")
            {

                var documentPurpose = $"{purpose} по дог.№ {documentId}, {studentFullName}, гр.№ {group}";
                var qrData = $"ST00012|Name=Министерство финансов Омской области (БПОУ «Омавиат» л/с 010220608)|PersonalAcc=03224643520000005201|BankName=Отделение Омск Банка России//УФК по Омской области г. Омск|BIC=015209001|CorrespAcc=0|PayeeINN=5504000055|KPP=550401001|" +
                    $"Purpose={documentPurpose}|Contract={documentId}|ChildFio={studentFullName}|Sum={summa * 100}" +
                    $"|CBC=01000000000000000130|OKTMO=52701000|CATEGORY=1";
                var qr = QrCode.EncodeText(qrData, QrCode.Ecc.Medium);
                string svg = qr.ToSvgString(4);
                System.IO.File.WriteAllText("hello-world-qr.svg", svg, Encoding.UTF8);
            }
        }
    }
}
