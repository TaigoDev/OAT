using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Runtime.InteropServices;

namespace OAT
{
    public class RawJsonBodyInputFormatter : InputFormatter
    {
        public RawJsonBodyInputFormatter()
        {
            SupportedMediaTypes.Add("application/json");
        }

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            using (var reader = new StreamReader(request.Body))
            {
                var content = await reader.ReadToEndAsync();
                return await InputFormatterResult.SuccessAsync(content);
            }
        }

        protected override bool CanReadType(Type type)
        {
            return type == typeof(string);
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
}
