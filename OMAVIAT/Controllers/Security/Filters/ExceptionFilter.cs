using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.InteropServices;

public class ExceptionFilter : IExceptionFilter {
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
