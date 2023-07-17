using Microsoft.AspNetCore.Authentication.Cookies;
using OAT;
using System.Runtime.InteropServices;
using static ProxyController;

config = Utils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "config.yml"), new Config());

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    config.MainUrl = "https://new.oat.ru";
    config.BaseUrl = "http://10.24.2.13:8082";
    config.bind_port = 20045;
    Console.WriteLine("Autocorrecting of config...");
}

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
    Utils.CreateDirectory(
    Path.Combine(Directory.GetCurrentDirectory(), "bitrix"),
    Path.Combine(Directory.GetCurrentDirectory(), "news"),
    Logger.path,
    Logger.path_PreventedAttempts);
    NewsController.init();
    UrlsContoller.init();
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
        options.AccessDeniedPath = "/panel/authorization";
        options.LoginPath = "/panel/authorization";
        options.Cookie.Name = "Authorization";
    });
    builder.Services.AddAuthorization();
    builder.Services.AddMvc(options => options.InputFormatters.Insert(0, new RawJsonBodyInputFormatter()));
    builder.WebHost.UseUrls($"http://0.0.0.0:{config.bind_port}");
}

async Task Proxing(HttpContext context, Func<Task> next)
{
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