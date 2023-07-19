using Microsoft.AspNetCore.Authentication.Cookies;
using MySqlConnector;
using OAT;
using Recovery.Tables;
using RepoDb;
using System.Runtime.InteropServices;
using TAIGO.ZCore.DPC.Services;
using static ProxyController;

config = Utils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "config.yml"), new Config());
if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    config.MainUrl = "https://www.oat.ru";
    config.BaseUrl = "http://10.24.2.13:8082";
    config.bind_port = 20045;
    Console.WriteLine("Autocorrecting of config...");
}
GlobalConfiguration.Setup().UseMySqlConnector();
Console.WriteLine(Utils.GetConnectionString());

var builder = WebApplication.CreateBuilder(args);
SetupServices(ref builder);
SetupControllers();
var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.UseRouting();
app.UseNoSniffHeaders();
app.Use((context, next) => Proxing(context, next));
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();


void SetupControllers()
{
    HealthTables.init();
    Utils.CreateDirectory(
    Path.Combine(Directory.GetCurrentDirectory(), "bitrix"),
    Path.Combine(Directory.GetCurrentDirectory(), "news"),
    Logger.path,
    Logger.path_PreventedAttempts);
    NewsController.init();
    UrlsContoller.init();
    OAT.Telegram.init();
    CreateAdminAccount();
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
    builder.Services.AddMvc(options => options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter()));
    builder.WebHost.UseUrls($"http://0.0.0.0:{config.bind_port}");
}

async Task Proxing(HttpContext context, Func<Task> next)
{
    try
    {
        var OnNewSite = UrlsContoller.Redirect(context.Request.Path.Value!);
        if (OnNewSite != null && $"/{OnNewSite}" != context.Request.Path.Value!)
        {
            context.Response.Redirect($"{config.MainUrl}/{OnNewSite}");
            return;
        }
        await next();
    }
    catch(Exception ex) 
    {
        Logger.Error(ex.ToString());
    }
    if (context.Response.StatusCode == 404)
        await context.DisplayBitrix(next);

}

async void CreateAdminAccount()
{
    try
    {
        using var connection = new MySqlConnection(Utils.GetConnectionString());
        var records = await connection.QueryAsync<users>(e => e.username == "admin");
        if (records.Any())
            return;
        await connection.InsertAsync(new users(
            "ќмский авиационный колледж",
            "admin",
            Utils.GetSHA256("v~S6pRKEX$}U@IPw"),
            Enums.Role.admin));
    }
    catch(Exception ex)
    {
        Console.WriteLine(ex);
    }
}