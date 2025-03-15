using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;

namespace _250310_vs_MiddlewareExample.CustomMiddleware;



// You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
public class HelloCustomMiddleware
{
    private readonly RequestDelegate _next;

    public HelloCustomMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        //befor logic
        if (httpContext.Request.Query.ContainsKey("firstName") &&
            httpContext.Request.Query.ContainsKey("lastName")
        )
        {
            var fullName = httpContext.Request.Query["firstName"] + " " + httpContext.Request.Query["lastName"];
            await httpContext.Response.WriteAsync($"Hello {fullName}!");
        }
        await _next(httpContext);
    }
}
// Extension method used to add the middleware to the HTTP request pipeline.
public static class HelloCustomModdleExtensions
{
    public static IApplicationBuilder UseHelloCustomMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<HelloCustomMiddleware>();
    }
}