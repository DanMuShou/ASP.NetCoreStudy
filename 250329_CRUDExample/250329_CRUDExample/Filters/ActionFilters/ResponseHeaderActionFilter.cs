using Microsoft.AspNetCore.Mvc.Filters;

namespace _250329_CRUDExample.Filters.ActionFilters;

public class ResponseHeaderFilterFactoryAttribute : Attribute, IFilterFactory
{
    public bool IsReusable => false;

    private string? Key { get; set; }
    private string? Value { get; set; }
    private int Order { get; set; }

    public ResponseHeaderFilterFactoryAttribute(string key, string value, int order)
    {
        Key = key;
        Value = value;
        Order = order;
    }

    public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
    {
        // 创建并返回过滤器对象
        //如果ResponseHeaderActionFilter的构造函数需要参数（如依赖注入的服务），则直接new会引发错误。
        //无法通过依赖注入（DI）自动注入依赖项（如ILogger等）。
        // var filter = new ResponseHeaderActionFilter();

        //通过服务提供器（DI容器）获取ResponseHeaderActionFilter的实例。
        //如果过滤器已注册为服务（如services.AddScoped<ResponseHeaderActionFilter>()），会返回一个已初始化的实例。
        var filter = serviceProvider.GetRequiredService<ResponseHeaderActionFilter>();
        filter.Key = Key;
        filter.Value = Value;
        filter.Order = Order;
        return filter;
    }
}

//Controller -> FilterFactory -> Filter
public class ResponseHeaderActionFilter(ILogger<ResponseHeaderActionFilter> logger)
    : IAsyncActionFilter,
        IOrderedFilter
{
    public string Key { get; set; }
    public string Value { get; set; }
    public int Order { get; set; }

    //IAsyncActionFilter, IOrderedFilter
    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next
    )
    {
        logger.LogInformation("Before logic OnActionExecutionAsync");
        await next();
        context.HttpContext.Response.Headers.Append(Key, Value);

        logger.LogInformation("after logic OnActionExecutionAsync");
    }
}
