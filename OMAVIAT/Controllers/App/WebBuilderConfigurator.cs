using Microsoft.AspNetCore.Authentication.Cookies;
using MyOAT.Utilities.Cookies;
using OMAVIAT.Controllers.Security;

namespace OMAVIAT.Controllers.App
{
	public class WebBuilderConfigurator
	{
		public static void SetupServices(ref WebApplicationBuilder builder)
		{

			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents(); builder.Services.AddControllersWithViews();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddServerSideBlazor(o =>o.DetailedErrors = true);
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
			builder.WebHost.UseUrls($"http://0.0.0.0:{Configurator.config.bind_port}");
			builder.Services.AddRazorComponents().AddInteractiveServerComponents();
			builder.Services.AddControllersWithViews();
			builder.Services.AddMvc().AddRazorPagesOptions(opt => opt.RootDirectory = "/RazorPages");


		}
	}
}
