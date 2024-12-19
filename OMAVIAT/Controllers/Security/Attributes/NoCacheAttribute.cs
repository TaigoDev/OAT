using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NoCacheAttribute : ActionFilterAttribute {
	public override void OnResultExecuting(ResultExecutingContext filterContext)
	{
		filterContext.HttpContext.Response.GetTypedHeaders().CacheControl =
			new CacheControlHeaderValue()
			{
				NoCache = true,
				NoStore = true,
				MaxAge = TimeSpan.FromHours(0),
			};
		base.OnResultExecuting(filterContext);
	}
}
