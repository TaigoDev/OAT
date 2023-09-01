using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Net.Http.Headers;
using OAT.Controllers;
using System.Runtime.InteropServices;

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
            var authResult = await AuthorizationController.ValidateCredentials(context.HttpContext.User, context.HttpContext.UserIP());
            if (authResult is Enums.AuthResult.success)
                await next();
            else
                context.Result = authResult is Enums.AuthResult.token_expired ? new StatusCodeResult(401) : new StatusCodeResult(403);
        }
        else
            await next();


    }
}