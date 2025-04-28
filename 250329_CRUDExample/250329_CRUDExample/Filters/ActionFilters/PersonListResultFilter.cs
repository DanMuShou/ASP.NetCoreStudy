using Microsoft.AspNetCore.Mvc.Filters;

namespace _250329_CRUDExample.Filters.ActionFilters;

public class PersonListResultFilter(ILogger<PersonListResultFilter> logger) : IAsyncResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next
    )
    {
        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(PersonListResultFilter),
            nameof(OnResultExecutionAsync)
        );

        context.HttpContext.Response.Headers.LastModified = DateTime.UtcNow.ToString(
            "yy-MM-dd HH:mm"
        );

        //响应头修改时机：必须在 await next() 之前 完成，确保响应未开始
        await next();

        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(PersonListResultFilter),
            nameof(OnResultExecutionAsync)
        );

        logger.LogInformation("响应状态: {status}", context.HttpContext.Response.HasStarted);
    }
}
