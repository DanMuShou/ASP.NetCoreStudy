using _250310_vs_MiddlewareExample.CustomMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomMiddleware>();
var app = builder.Build();

app.Use(async (HttpContext context, RequestDelegate next) =>
{
    await context.Response.WriteAsync("From Midleware 1\n");
    await next(context);
});

app.UseHelloCustomMiddleware();

app.Run(async context =>
{
    await context.Response.WriteAsync("From Middleware 3\n");
});

app.Run();

// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Hello");
// });
// app.Run(async context =>
// {
//     await context.Response.WriteAsync("Hello again");
// });
//middlware 1