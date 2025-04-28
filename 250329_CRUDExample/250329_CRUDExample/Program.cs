using _250329_CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Repositories;
using RepositoryContracts;
using Serilog;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddTransient<ResponseHeaderActionFilter>();

//注册 MVC 控制器和服务，使应用程序支持控制器（Controllers）和 Razor 视图（Views）功能。
builder.Services.AddControllersWithViews(options =>
{
    //将 ResponseHeaderActionFilter 全局添加到所有控制器或动作中。
    // options.Filters.Add<ResponseHeaderActionFilter>();
    var logger = builder
        .Services.BuildServiceProvider()
        .GetRequiredKeyedService<ILogger<ResponseHeaderActionFilter>>(null);

    var filter = new ResponseHeaderActionFilter(logger)
    {
        Key = "My-Key-Global",
        Value = "My-Value-Global",
        Order = 2,
    };
    options.Filters.Add(filter);
});

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

//Services.Add... 依赖注入（Dependency Injection, DI）容器注册服务
builder.Services.AddScoped<ICountriesRepository, CountriesRepository>();
builder.Services.AddScoped<IPersonsRepository, PersonsRepository>();
builder.Services.AddScoped<ICountriesService, CountriesService>();
builder.Services.AddScoped<IPersonsService, PersonsService>();

//每次从DI容器请求PersonListActionFilter实例时，都会创建一个全新实例
//适合轻量级无状态对象，资源消耗最低
builder.Services.AddTransient<PersonListActionFilter>();

//AddHttpLogging 方法用于在 ASP.NET Core 应用中添加 HTTP 请求和响应的日志记录功能。通过配置 options.LoggingFields，可以指定需要记录的请求或响应字段。
builder.Services.AddHttpLogging(options =>
    options.LoggingFields =
        HttpLoggingFields.RequestProperties | HttpLoggingFields.RequestPropertiesAndHeaders
);

if (!builder.Environment.IsEnvironment("Test"))
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    });
    Rotativa.AspNetCore.RotativaConfiguration.Setup("wwwroot", wkhtmltopdfRelativePath: "Rotativa");
}

var app = builder.Build();

if (builder.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseRouting();
app.MapControllers();
app.UseStaticFiles();

//提供标准化的日志记录功能，专注于记录 HTTP 请求和响应的详细信息（如请求头、路径、查询字符串等）。
app.UseHttpLogging();

//通过 Serilog 实现自定义化的请求日志记录，通常结合结构化日志（Structured Logging）功能使用。
app.UseSerilogRequestLogging();

app.Run();

public partial class Program { }

//21 - 20 Start
