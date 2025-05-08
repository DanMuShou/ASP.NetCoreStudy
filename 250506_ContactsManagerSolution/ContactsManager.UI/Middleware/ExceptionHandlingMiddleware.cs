namespace ContactsManagerSolution.Middleware;

public class ExceptionHandlingMiddleware(
    RequestDelegate next,
    ILogger<ExceptionHandlingMiddleware> logger
)
{
    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await next(httpContext);
        }
        catch (Exception ex)
        {
            if (ex.InnerException != null)
            {
                logger.LogError(
                    "{ExceptionType} : {ExceptionMessage}",
                    ex.InnerException.GetType().ToString(),
                    ex.InnerException.Message
                );
            }
            else
            {
                logger.LogError(
                    "{ExceptionType} : {ExceptionMessage}",
                    ex.GetType().ToString(),
                    ex.Message
                );
            }
            // httpContext.Response.StatusCode = 500;
            // await httpContext.Response.WriteAsync("发生错误 ERROR", Encoding.UTF8);
            throw;
        }
    }
}

public static class ExceptionHandlingMiddlewareExtensions
{
    public static IApplicationBuilder UseExceptionHandlingMiddleware(
        this IApplicationBuilder builder
    )
    {
        return builder.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
