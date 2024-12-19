using Microsoft.Net.Http.Headers;

namespace OMAVIAT.Controllers.App {
	public class CacheController {
		public static async Task Setup(HttpContext context, Func<Task> next)
		{
			if (context.Request.Path.Value!.Contains("blazor.server.js"))
			{
				context.Response.GetTypedHeaders().CacheControl =
					new CacheControlHeaderValue()
					{
						Public = true,
						MaxAge = TimeSpan.FromDays(7),
					};
				await next();
				return;
			}
			if (!context.Request.Path.Value!.Contains("admin") && !context.Request.Path.Value!.Contains("blazor"))
			{
				context.Response.GetTypedHeaders().CacheControl =
					new CacheControlHeaderValue()
					{
						NoCache = true,
						NoStore = true,
						MaxAge = TimeSpan.FromHours(0),
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
	}
}
