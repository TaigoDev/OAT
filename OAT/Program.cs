using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using OAT.Readers;
using OAT.Utilities;
using RepoDb;
using System.Runtime.InteropServices;
using TAIGO.ZCore.DPC.Services;
using static ProxyController;

config = Utils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(),
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "config.yml" : "config-linux.yml"), new Config());
GlobalConfiguration.Setup().UseMySqlConnector();
Console.WriteLine($"bu - {config.BaseUrl}; mu - {config.MainUrl}");
var builder = WebApplication.CreateBuilder(args);
SetupServices(ref builder);
SetupControllers();

var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "Resources", "static"))
});
app.UseRouting();
app.UseNoSniffHeaders();
app.Use((context, next) => Proxing(context, next));
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();



void SetupControllers()
{
    Console.WriteLine(Utils.GetConnectionString());
    try
    {
        Utils.CreateDirectoriesWithCurrentPath(
            "bitrix",
            "news",
            "Resources",
            "Resources/sessions", "Resources/sessions/b1", "Resources/sessions/b2", "Resources/sessions/b3", "Resources/sessions/b4",
            "Resources/teachers",
            "Resources/schedule",
            "Resources/pay",
            "Resources/static",
            Logger.path,
            Logger.path_PreventedAttempts);
        var contra = new Func<Task>(async () => await ContractReader.init());
        /* WARNING: Not support async methods */
        RunModules.StartModules(
            OAT.Utilities.Telegram.init,
            UrlsContoller.init,
            HealthTables.init,
            NewsReader.init,
            ProfNewsReader.init,
            ScheduleReader.init,
            ContractReader.init);
    }
    catch (Exception ex)
    {
        Logger.Error(ex.ToString());
    }
}

void SetupServices(ref WebApplicationBuilder builder)
{

    builder.Services.AddRazorPages();
    builder.Services.AddControllersWithViews();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(31);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/admin/authorization";
        options.LoginPath = "/admin/authorization";
        options.Cookie.Name = "Authorization";
    });
    builder.Services.AddAuthorization();
    builder.Services.AddMvc(options =>
    {
        options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter());
        options.Filters.Add<ExceptionFilter>();
        options.Filters.Add<ValidationFilter>();

    });
    builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));
    builder.WebHost.UseUrls($"http://0.0.0.0:{config.bind_port}");

}

async Task Proxing(HttpContext context, Func<Task> next)
{
    try
    {
        if (!context.Request.Path.Value!.Contains("admin"))
            context.Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue()
                {
                    Public = true,
                    MaxAge = TimeSpan.FromHours(2),
                };
        else
            context.Response.GetTypedHeaders().CacheControl =
                new CacheControlHeaderValue()
                {
                     NoCache = true,
                     NoStore = true,
                     MaxAge = TimeSpan.FromHours(0),
                };

        var OnNewSite = UrlsContoller.Redirect(context.Request.Path.Value!);
        if (OnNewSite != null && $"/{OnNewSite}" != context.Request.Path.Value!)
        {
            context.Response.Redirect($"{config.MainUrl}/{OnNewSite}");
            return;
        }
        await next();
        if (context.Response.StatusCode == 404)
            await context.DisplayBitrix(next);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
        if (!context.Response.HasStarted)
            context.Response.Redirect("https://www.oat.ru/");
    }
}

//async Task CreateAdminAccount()
//{
//    using var connection = new MySqlConnection(Utils.GetConnectionString());
//    var records = await connection.QueryAsync<users>(e => e.username == "admin");
//    if (records.Any())
//        return;
//    await connection.InsertAsync(new users(
//        "Омский авиационный колледж",
//        "admin",
//        Utils.GetSHA256("v~S6pRKEX$}U@IPw"),
//        Enums.Role.admin, Enums.Building.all));
//}


