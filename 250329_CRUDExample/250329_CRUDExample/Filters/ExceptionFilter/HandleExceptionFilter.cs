using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace _250329_CRUDExample.Filters.ExceptionFilter;

public class HandleExceptionFilter(
    ILogger<HandleExceptionFilter> logger,
    IHostEnvironment hostEnvironment
) : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        logger.LogError(
            "过滤调用: {FilterName} -> {FilterAction}\n MessageType{ExceptionType} Message{ExceptionMessage}",
            nameof(HandleExceptionFilter),
            nameof(OnException),
            context.Exception.GetType().ToString(),
            context.Exception.Message
        );

        if (hostEnvironment.IsDevelopment())
        {
            context.Result = new ContentResult()
            {
                Content = context.Exception.Message,
                StatusCode = 500,
            };
        }
    }
}
