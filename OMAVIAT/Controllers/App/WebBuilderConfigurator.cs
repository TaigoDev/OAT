using Microsoft.AspNetCore.Authentication.Cookies;
using MyOAT.Utilities.Cookies;
using NLog;
using OMAVIAT.Controllers.Security;
using OMAVIAT.Services;
using LogLevel = NLog.LogLevel;

namespace OMAVIAT.Controllers.App;

public class WebBuilderConfigurator
{
	public static void SetupServices(ref WebApplicationBuilder builder)
	{
		LogManager.Setup().SetupExtensions(ext => ext.RegisterTarget<TelegramLogTarget>()).LoadConfiguration(nlog =>
		{
			nlog.ForLogger().WriteToConsole();
			nlog.ForLogger().WriteTo(new TelegramLogTarget()).WithAsync();
			nlog.ForLogger()
				.WriteToFile(Path.Combine(Directory.GetCurrentDirectory(), "Logs", "log-${shortdate}.log"));
		});
		builder.Services.AddRazorComponents()
			.AddInteractiveServerComponents();
		builder.Services.AddControllersWithViews();
		builder.Services.AddEndpointsApiExplorer();
		builder.Services.AddServerSideBlazor(o => o.DetailedErrors = true);
		builder.Services.AddScoped<ICookie, Cookie>();

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
		builder.Services.AddHttpClient();
		builder.Services.AddHttpContextAccessor();
		builder.WebHost.UseUrls($"http://0.0.0.0:{Configurator.Config.BindPort}");
		builder.Services.AddRazorComponents().AddInteractiveServerComponents();
		builder.Services.AddControllersWithViews();
		builder.Services.AddMvc().AddRazorPagesOptions(opt => opt.RootDirectory = "/RazorPages");
	}
}