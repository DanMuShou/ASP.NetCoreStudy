using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagerSolution.Filters.ResourceFilter;

// 资源级过滤（如事务管理、请求拦截）
public class FeatureDisableResourceFilter(
    ILogger<FeatureDisableResourceFilter> logger,
    bool isDisable
) : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next
    )
    {
        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(FeatureDisableResourceFilter),
            nameof(OnResourceExecutionAsync)
        );

        if (isDisable)
        {
            // 强制返回 HTTP 404 状态码（Not Found），表示请求的资源不存在
            // 通过设置 context.Result，跳过后续的 await next()（即不再执行控制器方法、操作过滤器等）
            // context.Result = new NotFoundResult();

            //返回 HTTP 501 状态码：表示服务器 不支持实现请求的功能（Not Implemented）
            context.Result = new StatusCodeResult(StatusCodes.Status501NotImplemented);
            return;
        }
        else
        {
            await next();
        }

        logger.LogInformation(
            "过滤调用: {FilterName} -> {FilterAction}",
            nameof(FeatureDisableResourceFilter),
            nameof(OnResourceExecutionAsync)
        );
    }
}
