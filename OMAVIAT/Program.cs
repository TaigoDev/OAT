using OAT.Controllers.App;
using OAT.Controllers.Bitrix.Controllers;
using OAT.Controllers.Schedules.Readers;
using OAT.Utilities;
using OAT;
using OMAVIAT.Components;
using OAT.Utilities.Telegram;
using OfficeOpenXml;
using OMAVIAT.Services.Workers;
using OMAVIAT.Services.Payments;
using OMAVIAT.Services.News;
using OMAVIAT.Services.Schedule.MainSchedule;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
Runs.StartModules(
		//Получаем информация из конфигов
		DirectoriesConfigurator.Create,
        Configurator.init,

	   //Запускаем сервис ТГ
	   TelegramBot.init,
	   DownDetector.init, 
	   
	   //Запускаем базу данных 
	   DatabaseHelper.WaitStableConnection,

	   //Настраиваем переадресацию с битрикса
	   UrlsContoller.init,

	   //Запускаем новости
	   NewsReader.init,
	   ProfNewsReader.init,
	   DemoExamsNewsReader.init,
	   ScheduleReader.ReadAllAsync,

	   //Запускаем другие службы
	   () => RepeaterUtils.RepeatAsync(async () => await ContractReader.init(), 15),
	   WorkersReader.init
);
Runs.InThread(async () => await ChangesController.init());
var builder = WebApplication.CreateBuilder(args);
WebBuilderConfigurator.SetupServices(ref builder);
var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.UseNoSniffHeaders();
app.Use(CacheController.Setup);

if (Configurator.config.bitrixProxy)
    app.BitrixProxy();
else
{
    app.Use(async (context, next) =>
    {
        await next();

        if (context.Response.StatusCode == 404)
            if (context.Request.Path == "/admin/panel")
                context.Response.Redirect("/admin/news");
            else
                context.Response.Redirect("/Duck");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();
app.Run();
