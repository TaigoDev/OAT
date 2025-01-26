using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OMAVIAT;
using OMAVIAT.Controllers.Security.Controllers;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Utilities;

public class ValidationFilterForPages : IAsyncPageFilter
{
	public async Task OnPageHandlerExecutionAsync(PageHandlerExecutingContext context,
		PageHandlerExecutionDelegate next)
	{
		if (!context.RouteData.Values.Any(e =>
			    e.Value != null && e.Value.ToString()!.ToLower().Contains("admin") &&
			    !e.Value.ToString()!.ToLower().Contains("authorization")))
		{
			await next();
			return;
		}

		if (context.HttpContext.User == null || !context.HttpContext.User.Identity!.IsAuthenticated)
		{
			context.HttpContext.Response.Redirect("/admin/authorization");
			await next();
			return;
		}

		using var connection = new DatabaseContext();
		var token = await connection.Tokens.FirstOrDefaultAsync(e => e.Token == context.HttpContext.User.GetToken());
		var authResult =
			await AuthorizationController.ValidateCredentials(context.HttpContext.User, context.HttpContext.UserIP());
		if (authResult is AuthResult.success && token != null &&
		    token.username == context.HttpContext.User.GetUsername())
		{
			var identity = (ClaimsIdentity)context.HttpContext.User.Identity;
			var claims = identity.Claims.Where(e => e.Type == "Role");
			foreach (var claim in claims)
				identity.RemoveClaim(claim);

			var db_roles = JsonConvert.DeserializeObject<List<Role>>(token.Roles);
			foreach (var role in db_roles ?? [])
				identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
			await next();
		}
		else
		{
			context.HttpContext.Response.Redirect("/api/logout?type=token_expired");
			await next();
		}
	}

	public Task OnPageHandlerSelectionAsync(PageHandlerSelectedContext context)
	{
		return Task.CompletedTask;
	}
}