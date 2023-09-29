using AspNetCore.ReCaptcha;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.FileProviders;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using OAT.Readers;
using OAT.Utilities;
using RepoDb;
using System.Runtime.InteropServices;
using System.Text;
using TAIGO.ZCore.DPC.Services;
using static ProxyController;

config = Utils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(),
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "config.yml" : "config-linux.yml"), new Config());

GlobalConfiguration.Setup().UseMySqlConnector();
Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); //https://www.nuget.org/packages/ExcelDataReader/3.7.0-develop00385#readme-body-tab

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



async void SetupControllers()
{
    Console.WriteLine(Utils.GetConnectionString());
    try
    {
        await DropTokens();
        Utils.CreateDirectoriesWithCurrentPath(
            "news",
            "Resources",
            "Resources/sessions", "Resources/sessions/b1", "Resources/sessions/b2", "Resources/sessions/b3", "Resources/sessions/b4",
            "Resources/teachers",
            "Resources/schedule",
            "Resources/journal",
            "Resources/pay",
            "Resources/static",
            "Resources/bitrix",
            "Resources/practice", "Resources/practice/b1", "Resources/practice/b2", "Resources/practice/b3", "Resources/practice/b4",
            "Resources/Logs");
        /* WARNING: Not support async methods */
        RunModules.StartModules(
            OAT.Utilities.TelegramBot.init,
            UrlsContoller.init,
            HealthTables.init,
            NewsReader.init,
            ProfNewsReader.init,
            ScheduleReader.init,
            () => Utils.AutoRepeat(async () => await ContractReader.init(), 15));

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
        options.Filters.Add<ValidationFilterForPages>();

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



async Task DropTokens()
{
    try
    {
        using var connection = new MySqlConnection(Utils.GetConnectionString());
        await connection.ExecuteNonQueryAsync($"DROP TABLE Tokens;");
    }
    catch { }
}