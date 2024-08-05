using OfficeOpenXml;
using OMAVIAT;
using OMAVIAT.Components;
using OMAVIAT.Controllers.App;
using OMAVIAT.Controllers.Bitrix.Controllers;
using OMAVIAT.Services.News;
using OMAVIAT.Services.Payments;
using OMAVIAT.Services.Schedule.MainSchedule;
using OMAVIAT.Services.Schedule.ScheduleChanges;
using OMAVIAT.Services.Workers;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;

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
	   ChangesService.InitAsync,

	   //Запускаем другие службы
	   () => RepeaterUtils.RepeatAsync(async () => await ContractReader.init(), 15),
	   WorkersReader.init
);
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
