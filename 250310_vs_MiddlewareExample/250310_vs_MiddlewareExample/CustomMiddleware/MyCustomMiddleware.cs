namespace _250310_vs_MiddlewareExample.CustomMiddleware;

public class MyCustomMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await context.Response.WriteAsync("My Custom Middlewarea - Starts");
        await next(context);
        await context.Response.WriteAsync("My Custom Middlewarea - End");
    }
}

public static class CustomMiddlewareExtension
{
    public static IApplicationBuilder UseMyCustomMiddleware( this IApplicationBuilder app)
    {
        var app1 =  app.UseMiddleware<MyCustomMiddleware>();
        return app1;
    }
}