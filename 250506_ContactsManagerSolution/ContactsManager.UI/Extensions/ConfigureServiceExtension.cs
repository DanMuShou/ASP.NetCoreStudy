using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.DbContext;
using ContactsManager.Infrastructure.Repositories;
using ContactsManagerSolution.Filters.ActionFilters;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;

namespace ContactsManagerSolution.Extensions;

public static class ConfigureServiceExtension
{
    public static IServiceCollection ConfigureServices(
        this IServiceCollection service,
        IWebHostEnvironment builderEnvironment,
        ConfigurationManager builderConfiguration
    )
    {
        //注册 MVC 控制器和服务，使应用程序支持控制器（Controllers）和 Razor 视图（Views）功能。
        service.AddControllersWithViews(options =>
        {
            //将 ResponseHeaderActionFilter 全局添加到所有控制器或动作中。
            // options.Filters.Add<ResponseHeaderActionFilter>();
            var logger = service
                .BuildServiceProvider()
                .GetRequiredKeyedService<ILogger<ResponseHeaderActionFilter>>(null);

            var filter = new ResponseHeaderActionFilter(logger)
            {
                Key = "My-Key-Global",
                Value = "My-Value-Global",
                Order = 2,
            };
            options.Filters.Add(filter);
        });

        //Services.Add... 依赖注入（Dependency Injection, DI）容器注册服务
        service.AddScoped<ICountriesRepository, CountriesRepository>();
        service.AddScoped<IPersonsRepository, PersonsRepository>();

        service.AddScoped<IPersonsGetterService, PersonsGetterServiceWithFewExcelFields>();
        service.AddScoped<PersonsGetterService, PersonsGetterService>();

        service.AddScoped<IPersonsAdderService, PersonsAdderService>();
        service.AddScoped<IPersonsDeleterService, PersonsDeleterService>();
        service.AddScoped<IPersonsSorterService, PersonsSorterService>();
        service.AddScoped<IPersonsUpdaterService, PersonsUpdaterService>();

        service.AddScoped<ICountriesAdderService, CountriesAdderService>();
        service.AddScoped<ICountriesGetterService, CountriesGetterService>();
        service.AddScoped<ICountriesUploaderService, CountriesUploaderService>();


        //每次从DI容器请求PersonListActionFilter实例时，都会创建一个全新实例
        //适合轻量级无状态对象，资源消耗最低
        service.AddTransient<PersonListActionFilter>();
        service.AddTransient<ResponseHeaderActionFilter>();

        //AddHttpLogging 方法用于在 ASP.NET Core 应用中添加 HTTP 请求和响应的日志记录功能。通过配置 options.LoggingFields，可以指定需要记录的请求或响应字段。
        service.AddHttpLogging(options =>
            options.LoggingFields =
                HttpLoggingFields.RequestProperties | HttpLoggingFields.RequestPropertiesAndHeaders
        );

        if (!builderEnvironment.IsEnvironment("Test"))
        {
            service.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(builderConfiguration.GetConnectionString("DefaultConnection"));
            });
            Rotativa.AspNetCore.RotativaConfiguration.Setup(
                "wwwroot",
                wkhtmltopdfRelativePath: "Rotativa"
            );
        }

        return service;
    }
}