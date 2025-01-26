using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc.Filters;
using NLog;

public class ExceptionFilter : IExceptionFilter
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public void OnException(ExceptionContext context)
	{
		var actionName = context.ActionDescriptor.DisplayName;
		var exceptionStack = context.Exception.StackTrace;
		var exceptionMessage = context.Exception.Message;
		var pc = RuntimeInformation.IsOSPlatform(OSPlatform.Windows) ? "На компьютере разработчика" : "На сервере";
		Logger.Error($"{pc} произошла ошибка:\n" +
		             $"actionName: {actionName}\n" +
		             $"exceptionStack: {exceptionStack}\n" +
		             $"exceptionMessage: {exceptionMessage}");
		context.ExceptionHandled = false;
	}
}