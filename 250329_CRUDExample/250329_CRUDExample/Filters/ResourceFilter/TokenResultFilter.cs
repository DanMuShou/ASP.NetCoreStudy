using Microsoft.AspNetCore.Mvc.Filters;

namespace _250329_CRUDExample.Filters.ResourceFilter;

public class TokenResultFilter() : IAsyncResourceFilter
{
    public async Task OnResourceExecutionAsync(
        ResourceExecutingContext context,
        ResourceExecutionDelegate next
    )
    {
        context.HttpContext.Response.Cookies.Append("Auth-Key", "A100");
        await next();
    }
}
