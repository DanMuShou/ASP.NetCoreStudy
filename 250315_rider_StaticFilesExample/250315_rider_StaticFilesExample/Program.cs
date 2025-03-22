using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "myroot"
});
var app = builder.Build();

app.UseStaticFiles();//(myroot)
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(
        Path.Combine( builder.Environment.ContentRootPath , "myWebroot"))
});//(myWebroot)

app.UseRouting();
#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Hello World!");
    });
});
#pragma warning restore ASP0014


app.Run();