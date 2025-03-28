using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

var builder = WebApplication.CreateBuilder(args);

//使用Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();

// builder.Services.Add(
//     //ServiceLifetime.Transient - 每次都创建一个服务
//     //ServiceLifetime.Scoped - 每次客户端请求创建
//     //ServiceLifetime.Singleton - 单例
//     new ServiceDescriptor(typeof(ICitiesService), typeof(CitiesService), ServiceLifetime.Scoped)
// );
builder.Services.AddScoped<ICitiesService, CitiesService>();
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    //Add Transient
    // containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency();
    //Add Scoped
    containerBuilder
        .RegisterType<CitiesService>()
        .As<ICitiesService>()
        .InstancePerLifetimeScope();
    //Add Singleton
    // containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance();
});

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();
