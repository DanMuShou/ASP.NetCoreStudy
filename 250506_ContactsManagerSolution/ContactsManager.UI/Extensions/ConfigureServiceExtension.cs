using ContactsManager.Core.Domain.IdentityEntities;
using ContactsManager.Core.Domain.RepositoryContract;
using ContactsManager.Core.ServiceContracts;
using ContactsManager.Core.Services;
using ContactsManager.Infrastructure.AppDbContext;
using ContactsManager.Infrastructure.Repositories;
using ContactsManagerSolution.Filters.ActionFilters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ContactsManagerSolution.Extensions;

public static class ConfigureServiceExtension
{
    public static void ConfigureServices(
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

        service
            .AddAuthorizationBuilder()
            .SetFallbackPolicy(new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build())
            .AddPolicy(
                "NotAuthenticated",
                policy =>
                {
                    policy.RequireAssertion(context =>
                        context.User.Identity is not { IsAuthenticated: true }
                    );
                }
            );

        service.ConfigureApplicationCookie(options =>
        {
            options.LoginPath = "/Account/Login";
        });

        //这行代码向依赖注入容器注册了 ASP.NET Core Identity 系统。
        service
            //使用了自定义的 ApplicationUser（继承自 IdentityUser）和 ApplicationRole（继承自 IdentityRole）作为用户和角色实体类。
            .AddIdentity<ApplicationUser, ApplicationRole>(options =>
            {
                options.Password.RequireDigit = true; // 要求数字
                options.Password.RequireLowercase = true; // 要求小写字母
                options.Password.RequireUppercase = true; // 要求大写字母
                options.Password.RequireNonAlphanumeric = false; // 默认为 true，即要求非字母数字字符（符号）
                options.Password.RequiredLength = 8; // 密码最小长度
                options.Password.RequiredUniqueChars = 4; // 密码要求包含的字符数
            })
            //指定 Identity 系统将使用 Entity Framework Core 来持久化用户、角色等信息。
            .AddEntityFrameworkStores<ApplicationDbContext>()
            //添加默认的令牌提供程序 -> 用户注册时的电子邮件确认 密码重置 两步验证
            .AddDefaultTokenProviders()
            //明确指定使用 UserStore 泛型实现来管理用户数据。
            .AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
            //该行代码的作用是为 ASP.NET Core Identity 系统显式注册一个自定义的角色存储实现,用于管理角色（Role）数据的持久化操作。
            .AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();

        // ApplicationUser: 自定义用户实体。
        // ApplicationRole: 自定义角色实体。
        // ApplicationDbContext: 数据库上下文。
        // Guid: 用户主键类型（通常是 string 或 Guid）
    }
}
