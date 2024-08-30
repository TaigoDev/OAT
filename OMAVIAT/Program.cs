using OfficeOpenXml;
using OMAVIAT;
using OMAVIAT.Components;
using OMAVIAT.Controllers.App;
using OMAVIAT.Controllers.Bitrix.Controllers;
using OMAVIAT.Schedule;
using OMAVIAT.Services.News;
using OMAVIAT.Services.Payments;
using OMAVIAT.Services.Workers;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
await DirectoriesConfigurator.Create();
await Configurator.init();
await ScheduleLib.Init(TelegramBot.SendMessage, new OMAVIAT.Schedule.Entities.DatabaseConfig
{
	db_ip = Configurator.config.db_ip,
	db_name = Configurator.config.db_name,
	db_password = Configurator.config.db_password,
	db_port = Configurator.config.db_port,
	db_user = Configurator.config.db_user,
	redis_ip = Configurator.config.redis_ip,
	redis_port = Configurator.config.redis_port,
	redis_password = Configurator.config.redis_password,
	IsLocalDB = OperatingSystem.IsWindows(),
});

Runs.StartModules(

	   //Çàïóñêàåì ñåðâèñ ÒÃ
	   TelegramBot.init,
	   DownDetector.init,

	   //Çàïóñêàåì áàçó äàííûõ 
	   DatabaseHelper.WaitStableConnection,
	   async () => await ScheduleLib.Start(false, false),
	   //Íàñòðàèâàåì ïåðåàäðåñàöèþ ñ áèòðèêñà
	   UrlsContoller.init,

	   //Çàïóñêàåì íîâîñòè
	   NewsReader.init,
	   ProfNewsReader.init,
	   DemoExamsNewsReader.init,

	   //Çàïóñêàåì äðóãèå ñëóæáû
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
			context.Response.Redirect(context.Request.Path == "/admin/panel" ? "/admin/news" : "/Duck");
	});
}

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();
app.Run();
