using ConfigurationExample;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

//提供一个WeaterApi对象
builder.Services.Configure<WeatherApiOptions>(builder.Configuration.GetSection("WeatherApi"));
var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
;

app.Run();
