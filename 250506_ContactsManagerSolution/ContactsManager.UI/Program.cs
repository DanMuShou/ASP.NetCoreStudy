// UI 引用Core 引用
//

using ContactsManagerSolution.Extensions;
using ContactsManagerSolution.Middleware;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

//Serilog 配置
//Serilog 集成到 ASP.NET Core 应用中，替代默认的日志实现。
builder.Host.UseSerilog(
    (context, services, loggerConfiguration) =>
    {
        loggerConfiguration
            //从应用的配置文件（如 appsettings.json）中读取 Serilog 相关配置（如日志级别、输出路径等）。
            .ReadFrom.Configuration(context.Configuration)
            //通过 IServiceProvider（services 参数）从已注册的服务中读取配置或依赖项。
            .ReadFrom.Services(services);
    }
);

builder.Services.ConfigureServices(builder.Environment, builder.Configuration);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}

app.UseStaticFiles();

app.UseAuthentication(); // 启用身份验证中间件，用于验证用户身份（如 Cookie 认证、JWT 等）
app.UseRouting(); // 启用路由中间件，用于解析请求路径并映射到对应的控制器/动作方法
app.MapControllers(); // 将控制器（Controller）中的 API 路由注册到应用中，支持属性路由等机制

//提供标准化的日志记录功能，专注于记录 HTTP 请求和响应的详细信息（如请求头、路径、查询字符串等）。
app.UseHttpLogging();

//通过 Serilog 实现自定义化的请求日志记录，通常结合结构化日志（Structured Logging）功能使用。
app.UseSerilogRequestLogging();

app.Run();

partial class Program { }

//25 - 9
