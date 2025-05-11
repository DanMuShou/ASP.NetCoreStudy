using ContactsManager.Infrastructure.AppDbContext;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

namespace ContactsManager.IntegrationTests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        //将环境变量设为 "Test"，使应用程序在测试时加载特定的配置文件（如 appsettings.Test.json），与生产环境隔离。
        builder.UseEnvironment("Test");
        //：移除主应用程序中注册的 DbContextOptions<ApplicationDbContext> 服务。
        builder.ConfigureServices(services =>
        {
            var description = services.SingleOrDefault(temp =>
                temp.ServiceType == typeof(DbContextOptions<ApplicationDbContext>)
            );
            if (description != null)
            {
                services.Remove(description);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("DbForTesting")
            );
        });
    }
}
