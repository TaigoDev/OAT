using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using OAT;
using OAT.Modules.MNews.Readers;
using OAT.Modules.Payments.Readers;
using OAT.Modules.ReCaptchaV3;
using OAT.Modules.Recovery;
using OAT.Modules.Schedules.Readers;
using OAT.Modules.Security;
using OAT.Modules.Telegram;
using OAT.Modules.Workers;
using OAT.Utilities;
using OAT.UtilsHelper.ReCaptcha;
using OfficeOpenXml;
using RepoDb;

await Configurator.init();
GlobalConfiguration.Setup().UseMySqlConnector();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

var builder = WebApplication.CreateBuilder(args);
SetupServices(ref builder);
SetupControllers();

var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.MapBlazorHub();
app.UseRouting();
app.UseNoSniffHeaders();
app.Use((context, next) => CacheController(context, next));

if (Configurator.config.bitrixProxy)
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
var ok = 245;
Console.WriteLine(ok * ++ok);
app.UseCors("AllowAll");
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
			"Resources/schedule/latest",
			"Resources/journal",
			"Resources/pay",
			"Resources/static",
			"Resources/bitrix",
			"Resources/practice", "Resources/practice/b1", "Resources/practice/b2", "Resources/practice/b3", "Resources/practice/b4",
			"Resources/Logs");
		Runs.StartModules(
			TelegramBot.init,
			UrlsContoller.init,
			DropTokens,
			HealthTables.init,
			NewsReader.init,
			ProfNewsReader.init,
			DemoExamsNewsReader.init,
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
	builder.Services.AddServerSideBlazor(o => o.DetailedErrors = true);

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
	builder.Services.AddScoped(typeof(ICaptchaV3Validator), typeof(ReCaptchaV3Validator));
	builder.Services.AddHttpClient();
	builder.Services.AddHttpContextAccessor();
	builder.Services.AddReCaptcha(builder.Configuration.GetSection("ReCaptcha"));
	builder.WebHost.UseUrls($"http://0.0.0.0:{Configurator.config.bind_port}");

}


async Task CacheController(HttpContext context, Func<Task> next)
{
	if (!context.Request.Path.Value!.Contains("admin") && !context.Request.Path.Value!.Contains("blazor"))
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
