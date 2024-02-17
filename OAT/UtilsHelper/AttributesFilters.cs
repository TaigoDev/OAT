using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using MySqlConnector;
using Newtonsoft.Json;
using OAT.Controllers;
using OAT.Utilities;
using RepoDb;
using System.Runtime.InteropServices;
using System.Security.Claims;

[AttributeUsage(AttributeTargets.Class)]
public class MysqlTable : Attribute { }


[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class NoCacheAttribute : ActionFilterAttribute
{
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

public class ExceptionFilter : IExceptionFilter
{
	public void OnException(ExceptionContext context)
	{
		string? actionName = context.ActionDescriptor.DisplayName;
		string? exceptionStack = context.Exception.StackTrace;
		var exceptionMessage = context.Exception.Message;
		var pc = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ?
			"На компьютере разработчика" : "На сервере";
		Logger.Error($"{pc} произошла ошибка:\n" +
			$"actionName: {actionName}\n" +
			$"exceptionStack: {exceptionStack}\n" +
			$"exceptionMessage: {exceptionMessage}");
		context.ExceptionHandled = false;
	}


}

public class ValidationFilter : IAsyncActionFilter
{
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		var controllerActionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;
		if (controllerActionDescriptor == null)
		{
			await next();
			return;
		}

		var isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AuthorizeRolesAttribute)) || a.GetType().Equals(typeof(AuthorizeAttribute)));
		if (isDefined)
		{
			using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
			var token = (await connection.QueryAsync<Tokens>(e => e.Token == context.HttpContext.User.GetToken())).FirstOrDefault();
			var authResult = await AuthorizationController.ValidateCredentials(context.HttpContext.User, context.HttpContext.UserIP());

			if (authResult is Enums.AuthResult.success && token != null && token.username == context.HttpContext.User.GetUsername())
			{
				var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
				var claims = identity.Claims.Where(e => e.Type == "Role");
				foreach (var claim in claims)
					identity.RemoveClaim(claim);
				var db_roles = JsonConvert.DeserializeObject<List<Enums.Role>>(token.Roles);
				foreach (var role in db_roles)
					identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
				await next();
			}
			else
				context.Result = authResult is Enums.AuthResult.token_expired ? new StatusCodeResult(401) : new StatusCodeResult(403);
		}
		else
			await next();


	}
}

public class ValidationFilterForPages : IAsyncPageFilter
{
	public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context, PageHandlerExecutionDelegate next)
	{
		if (!context.RouteData.Values.Any(e => e.Value != null && e.Value.ToString()!.ToLower().Contains("admin") && !e.Value.ToString()!.ToLower().Contains("authorization")))
		{
			await next();
			return;
		}

		if (context.HttpContext.User == null || !context.HttpContext.User.Identity.IsAuthenticated)
		{
			context.HttpContext.Response.Redirect("/admin/authorization");
			await next();
			return;
		}

		using var connection = new MySqlConnection(DataBaseUtils.GetConnectionString());
		var token = (await connection.QueryAsync<Tokens>(e => e.Token == context.HttpContext.User.GetToken())).FirstOrDefault();
		var authResult = await AuthorizationController.ValidateCredentials(context.HttpContext.User, context.HttpContext.UserIP());
		if (authResult is Enums.AuthResult.success && token != null && token.username == context.HttpContext.User.GetUsername())
		{
			var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
			var claims = identity.Claims.Where(e => e.Type == "Role");
			foreach (var claim in claims)
				identity.RemoveClaim(claim);

			var db_roles = JsonConvert.DeserializeObject<List<Enums.Role>>(token.Roles);
			foreach (var role in db_roles)
				identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
			await next();
		}
		else
		{
			context.HttpContext.Response.Redirect("/api/logout?type=token_expired");
			await next();
			return;
		}
	}

	public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context) =>
		Task.CompletedTask;
}