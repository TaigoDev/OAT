using Controllers.BasicAuth;
using Microsoft.AspNetCore.Authentication.Cookies;
using OAT.function;
using System.Runtime.InteropServices;
using static ProxyController;

ProxyController.config = Utils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(), "config.yml"), new ProxyController.Config());

if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    config.MainUrl = "https://new.oat.ru";
    config.BaseUrl = "http://10.24.2.13:8082";
    config.bind_port = 20045;
    Console.WriteLine("Autocorrect config...");
}
var builder = WebApplication.CreateBuilder(args);
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
builder.WebHost.UseUrls($"http://0.0.0.0:{ProxyController.config.bind_port}");

if (!Directory.Exists(Path.Combine(Directory.GetCurrentDirectory(), "bitrix")))
    Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "bitrix"));
NewsController.init();
UrlsContoller.init();
var app = builder.Build();
Console.WriteLine($"Bitrix url: {ProxyController.config.BaseUrl}\nMain url: {ProxyController.config.MainUrl}\n 10.07.2023 12:56");

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.UseRouting();
app.UseNoSniffHeaders();
app.Use(async (context, next) => {
    var OnNewSite = UrlsContoller.Redirect(context.Request.Path.Value!);
    if (OnNewSite != null && $"/{OnNewSite}" != context.Request.Path.Value!)
    {
        context.Response.Redirect($"{ProxyController.config.MainUrl}/{OnNewSite}");
        return;
    }
    await next();
    if (context.Response.StatusCode == 404)
        await context.DisplayBitrix(next);
});

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();