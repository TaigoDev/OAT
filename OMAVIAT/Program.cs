using OfficeOpenXml;
using OMAVIAT;
using OMAVIAT.Components;
using OMAVIAT.Controllers.App;
using OMAVIAT.Controllers.Bitrix.Controllers;
using OMAVIAT.Schedule;
using OMAVIAT.Schedule.Entities;
using OMAVIAT.Services;
using OMAVIAT.Services.News;
using OMAVIAT.Services.Payments;
using OMAVIAT.Services.Workers;
using OMAVIAT.Utilities;
using OMAVIAT.Utilities.Telegram;
using Quartz.Logging;

LogProvider.IsDisabled = true;
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var builder = WebApplication.CreateBuilder(args);
WebBuilderConfigurator.SetupServices(ref builder);
await DirectoriesConfigurator.Create();
await Configurator.Init();
await ScheduleLib.Init(TelegramBot.SendMessage, new DatabaseConfig
{
	db_ip = Configurator.Config.Database.Address,
	db_name = Configurator.Config.Database.Name,
	db_password = Configurator.Config.Database.Password,
	db_port = Configurator.Config.Database.Port,
	db_user = Configurator.Config.Database.Username,
	IsLocalDB = !Configurator.Config.IsProduction
});

Runs.StartModules(
//Çàïóñêàåì ñåðâèñ ÒÃ
	TelegramBot.Init,
	DownDetector.Init,

//Çàïóñêàåì áàçó äàííûõ 
	DatabaseHelper.WaitStableConnection,
	async () => await ScheduleLib.Start(false, false, false,
		false, false),
//Íàñòðàèâàåì ïåðåàäðåñàöèþ ñ áèòðèêñà
	UrlsContoller.init,

//Çàïóñêàåì íîâîñòè
	NewsReader.Init,
	ScheduleService.RegisterAsync,
	ProfNewsReader.Init,
	DemoExamsNewsReader.Init,
	MuseumNewsReader.Init,

//Çàïóñêàåì äðóãèå ñëóæáû
	() => RepeaterUtils.RepeatAsync(async () => await ContractReader.Init(), 15),
	WorkersReader.Init
);

var app = builder.Build();
app.UseStaticFiles();
app.UseRouting();
app.UseAntiforgery();
app.UseNoSniffHeaders();
app.Use(CacheController.Setup);

if (Configurator.Config.BitrixProxy)
	app.BitrixProxy();
else
	app.Use(async (context, next) =>
	{
		await next();

		if (context.Response.StatusCode == 404)
			context.Response.Redirect(context.Request.Path == "/admin/panel" ? "/admin/news" : "/Duck");
	});

app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllerRoute("default", "{controller=Home}/{action=Get}/{id?}");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();
await app.RunAsync();
await ScheduleService.ShutdownAsync();