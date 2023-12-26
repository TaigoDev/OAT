using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using OAT.Readers;
using OAT.Utilities;
using OAT.UtilsHelper;
using OAT.UtilsHelper.ReCaptcha;
using OAT.UtilsHelper.Telegram;
using OfficeOpenXml;
using RepoDb;
using System.Runtime.InteropServices; 
using TAIGO.ZCore.DPC.Services;
using static ProxyController;

config = await FileUtils.SetupConfiguration(Path.Combine(Directory.GetCurrentDirectory(),
    RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "config.yml" : "config-linux.yml"), new Config());
GlobalConfiguration.Setup().UseMySqlConnector();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var builder = WebApplication.CreateBuilder(args);
SetupServices(ref builder);
SetupControllers();
var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.UseRouting();
app.UseNoSniffHeaders();

app.Use((context, next) => CacheController(context, next));
if (config.bitrixProxy)
    app.BitrixProxy();
else
{
    app.Use(async (context, next) => 
    {
        await next();
        if (context.Response.StatusCode == 404)
            context.Response.Redirect("https://www.oat.ru/Duck");
    });
}

app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.Run();



void SetupControllers()
{
    try
    {
        DirectoryUtils.CreateDirectoriesWithCurrentPath(
            "news",
            "Resources",
            "Resources/sessions", "Resources/sessions/b1", "Resources/sessions/b2", "Resources/sessions/b3", "Resources/sessions/b4",
            "Resources/workers",
            "wwwroot/people",
            "Resources/schedule",
            "Resources/journal",
            "Resources/pay",
            "Resources/static",
            "Resources/bitrix",
            "Resources/practice", "Resources/practice/b1", "Resources/practice/b2", "Resources/practice/b3", "Resources/practice/b4",
            "Resources/Logs");
        /* WARNING: Not support async methods */
        Runs.StartModules(
            TelegramBot.init,
            TimeTableBot.init,
            UrlsContoller.init,
            DropTokens,
            HealthTables.init,
            NewsReader.init,
            ProfNewsReader.init,
            ScheduleReader.init,
            () => RepeaterUtils.RepeatAsync(async () => await ContractReader.init(), 15),
            WorkersReader.init);
        Runs.InThread(async () => await ChangesController.init());
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


async Task CacheController(HttpContext context, Func<Task> next)
{
    if (!context.Request.Path.Value!.Contains("admin"))
    {
        context.Response.GetTypedHeaders().CacheControl =
            new CacheControlHeaderValue()
            {
                Public = true,
                MaxAge = TimeSpan.FromHours(24),
            };
        await next();
        return;
    }

    context.Response.GetTypedHeaders().CacheControl =
            new CacheControlHeaderValue()
            {
                NoCache = true,
                NoStore = true,
                MaxAge = TimeSpan.FromHours(0),
            };
    await next();
}

async Task DropTokens()
{
    using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
    await connection.ExecuteNonQueryAsync($"DROP TABLE Tokens;");
}
