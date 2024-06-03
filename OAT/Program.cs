using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using OAT;
using OAT.Controllers.Bitrix.Controllers;
using OAT.Controllers.MNews.Readers;
using OAT.Controllers.Payments.Readers;
using OAT.Controllers.ReCaptchaV2;
using OAT.Controllers.ReCaptchaV3;
using OAT.Controllers.Schedules.Readers;
using OAT.Controllers.Security;
using OAT.Controllers.Workers;
using OAT.Utilities;
using OAT.Utilities.Telegram;
using OfficeOpenXml;
using RepoDb;

await Configurator.init();
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
var builder = WebApplication.CreateBuilder(args);
await TelegramBot.init();
await DatabaseHelper.WaitStableConnection();
using var db = new DatabaseContext();
Console.WriteLine($"Количество новостей: {db.News.Count()} ");
db.Database.Migrate();
SetupServices(ref builder);
SetupControllers();

var app = builder.Build();
app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Get}/{id?}");
app.UseStaticFiles();
app.MapBlazorHub(configureOptions: options =>
{
	options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
}); app.UseAntiforgery();
app.UseRouting();
app.UseNoSniffHeaders();
app.Use(CacheController);

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
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();
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
			"Resources/static", "Resources/static/documents",
			"Resources/bitrix",
			"Resources/practice", "Resources/practice/b1", "Resources/practice/b2", "Resources/practice/b3", "Resources/practice/b4",
			"Resources/Logs");
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
	catch (Exception ex)
	{
		Logger.Error(ex.ToString());
	}
}

void SetupServices(ref WebApplicationBuilder builder)
{

	builder.Services.AddRazorComponents()
		.AddInteractiveServerComponents(); builder.Services.AddControllersWithViews();
	builder.Services.AddEndpointsApiExplorer();
	builder.Services.AddServerSideBlazor(o => o.DetailedErrors = true);
	builder.Services.AddDistributedMemoryCache();

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
	builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
	builder.Services.AddSession();

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
	using var connection = new DatabaseContext();
	Console.WriteLine($"{await connection.Tokens.CountAsync()}");
	connection.RemoveRange(connection.Tokens);
	await connection.SaveChangesAsync();
}
