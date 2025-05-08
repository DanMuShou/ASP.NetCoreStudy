using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManagerSolution.Filters.ResourceFilter;

public class PersonAlwaysRunResultFilter : IAsyncAlwaysRunResultFilter
{
    public async Task OnResultExecutionAsync(
        ResultExecutingContext context,
        ResultExecutionDelegate next
    )
    {
        if (context.Filters.OfType<SkipFilter>().Any())
        {
            await next();
            return;
        }
        await next();
    }
}
