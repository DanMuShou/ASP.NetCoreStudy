using ModelValidationsExample.Custom;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers(
    // options => { options.ModelBinderProviders.Insert(0, new PersonBinderProvider()); }
);
builder.Services.AddConnections();
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();