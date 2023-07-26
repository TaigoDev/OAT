using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using MySqlConnector;
using OAT.Readers;
using OAT.Utilities;
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

var builder = WebApplication.CreateBuilder(args);
SetupServices(ref builder);
SetupControllers();

var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(Directory.GetCurrentDirectory(), "static"))
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
        HealthTables.init();
        Utils.CreateDirectory(
        Path.Combine(Directory.GetCurrentDirectory(), "bitrix"),
        Path.Combine(Directory.GetCurrentDirectory(), "news"),
        Path.Combine(Directory.GetCurrentDirectory(), "static"),
        Path.Combine(Directory.GetCurrentDirectory(), "static", "teachers"),
        Path.Combine(Directory.GetCurrentDirectory(), "schedule"),
        Logger.path,
        Logger.path_PreventedAttempts);
        NewsController.init();
        UrlsContoller.init();
        OAT.Utilities.Telegram.init();
        ScheduleReader.init();
        CommandsController.init();
        CreateAdminAccount();
    }
    catch(Exception ex)
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
    });
    builder.WebHost.UseUrls($"http://0.0.0.0:{config.bind_port}");
}

async Task Proxing(HttpContext context, Func<Task> next)
{
    try
    {
        var OnNewSite = UrlsContoller.Redirect(context.Request.Path.Value!);
        if (!context.Request.Path.ToString().Contains("admin"))
            context.Response.Headers.CacheControl = "public, max-age=14400";
        else
            context.Response.Headers.CacheControl = "no-cache, no-store";

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
        context.Response.Redirect("https://www.oat.ru/");
    }
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
            "������ ����������� �������",
            "admin",
            Utils.GetSHA256("v~S6pRKEX$}U@IPw"),
            Enums.Role.admin));
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex);
    }
}