using ServiceManagement.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// 添加服务交互
builder.Services.AddRazorComponents().AddInteractiveServerComponents();

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
