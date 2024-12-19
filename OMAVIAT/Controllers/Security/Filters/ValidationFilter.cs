using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OMAVIAT;
using OMAVIAT.Controllers.Security.Controllers;
using OMAVIAT.Entities.Enums;
using OMAVIAT.Utilities;
using System.Security.Claims;

public class ValidationFilter : IAsyncActionFilter {
	public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
	{
		if (context.ActionDescriptor is not ControllerActionDescriptor controllerActionDescriptor)
		{
			await next();
			return;
		}

		var isDefined = controllerActionDescriptor.MethodInfo.GetCustomAttributes(inherit: true).Any(a => a.GetType().Equals(typeof(AuthorizeRolesAttribute)) || a.GetType().Equals(typeof(AuthorizeAttribute)));
		if (isDefined)
		{
			using var connection = new DatabaseContext();
			var token = await connection.Tokens.FirstOrDefaultAsync(e => e.Token == context.HttpContext.User.GetToken());
			var authResult = await AuthorizationController.ValidateCredentials(context.HttpContext.User, context.HttpContext.UserIP());

			if (authResult is AuthResult.success && token != null && token.username == context.HttpContext.User.GetUsername())
			{
				var identity = (ClaimsIdentity)context.HttpContext.User.Identity!;
				var claims = identity.Claims.Where(e => e.Type == "Role");
				foreach (var claim in claims)
					identity.RemoveClaim(claim);
				var db_roles = JsonConvert.DeserializeObject<List<Role>>(token.Roles);
				foreach (var role in db_roles ?? [])
					identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));
				await next();
			}
			else
				context.Result = authResult is AuthResult.token_expired ? new StatusCodeResult(401) : new StatusCodeResult(403);
		}
		else
			await next();


	}
}
