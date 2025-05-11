using Microsoft.EntityFrameworkCore;
using ServiceManagement.Components;
using ServiceManagement.Data;
using ServiceManagement.Models;
using ServiceManagement.StateStore;

var builder = WebApplication.CreateBuilder(args);
var builderConfig = builder.Configuration;

//默认生命周期是 Scoped（每个请求一个实例）。
// builder.Services.AddDbContext<ServerManagementContext>(options =>
// {
//     options.UseNpgsql(builderConfig.GetConnectionString("DefaultConnection"));
// });

//用于按需创建上下文实例，适用于你需要手动控制上下文创建时机的场景，比如在后台任务、仓储模式或非依赖注入的类中使用。
//注入 IDbContextFactory<ServerManagementContext> 然后创建上下文：
builder.Services.AddDbContextFactory<ServerManagementContext>(options =>
{
    options.UseNpgsql(builderConfig.GetConnectionString("DefaultConnection"));
});

// Add services to the container.
// 添加服务交互
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

//添加生命周期
// builder.Services.AddTransient<SessionStorage>();
builder.Services.AddScoped<ContainerStorage>();
builder.Services.AddScoped<TorontoOnlineServersStore>();

//防止多线程导致的问题
builder.Services.AddTransient<IServersRepository, ServersRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

//强制将HTTP请求重定向到HTTPS，确保所有流量使用安全协议传输。
app.UseHttpsRedirection();

app.UseStaticFiles();

//启用抗跨站请求伪造（XSRF/CSRF）保护，通过验证请求中的AntiForgeryToken来防止恶意请求。
app.UseAntiforgery();

//配置Razor组件路由，指定App类作为根组件，用于托管Blazor Server或Interactive WebAssembly应用的路由和渲染。
app.MapRazorComponents<App>().AddInteractiveServerRenderMode();

app.Run();


//9 - 1
