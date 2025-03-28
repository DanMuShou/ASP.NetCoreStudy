using _250315_rider_ControllersExample.Controllers;


var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();  //添加所有带controller后缀的控制器

var app = builder.Build();

app.UseStaticFiles();

app.MapControllers();

// app.UseRouting();
//
// #pragma warning disable ASP0014
// app.UseEndpoints(endpoints =>
// {
//     endpoints.MapControllers();
// });
// #pragma warning restore ASP0014
// app.Run(async context =>
// {
//     context.Response.ContentType = "text/plain;charset=utf-8";
//     await context.Response.WriteAsync("default page - 没有找到任何页面 404 ");
// });

app.Run();