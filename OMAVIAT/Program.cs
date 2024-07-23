using OAT.Controllers.App;
using OAT.Controllers.Bitrix.Controllers;
using OAT.Controllers.MNews.Readers;
using OAT.Controllers.Schedules.Readers;
using OAT.Controllers.Workers;
using OAT.Utilities;
using OAT;
using OMAVIAT.Components;
using OAT.Controllers.Payments.Readers;
using Microsoft.EntityFrameworkCore;
using OAT.Utilities.Telegram;
using OfficeOpenXml;

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var builder = WebApplication.CreateBuilder(args);
await Configurator.init();

await TelegramBot.init();
DownDetector.init();
await DatabaseHelper.WaitStableConnection();
using var db = new DatabaseContext();
Console.WriteLine($"Количество новостей: {db.News.Count()} ");
WebBuilderConfigurator.SetupServices(ref builder);
SetupControllers();

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
app.MapBlazorHub();
app.UseAuthentication();
app.UseAuthorization();
app.UseCors("AllowAll");
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
app.MapRazorPages();
app.Run();
void SetupControllers()
{
    DirectoriesConfigurator.Create();
    Runs.StartModules(
        UrlsContoller.init,
        DropTokens,
        NewsReader.init,
        ProfNewsReader.init,
        DemoExamsNewsReader.init,
        ScheduleReader.init,
        () => RepeaterUtils.RepeatAsync(async () => await ContractReader.init(), 15),
        WorkersReader.init);
    Runs.InThread(async () => await ChangesController.init());
}

async Task DropTokens()
{
    using var connection = new DatabaseContext();
    Console.WriteLine($"{await connection.Tokens.CountAsync()}");
    connection.RemoveRange(connection.Tokens);
    await connection.SaveChangesAsync();
}
