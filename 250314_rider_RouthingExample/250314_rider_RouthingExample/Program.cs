using _250314_rider_RoutingExample.CustomConstraints;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRouting(options => { options.ConstraintMap.Add("months", typeof(MonthsCustomConstraint)); });

var app = builder.Build();

app.UseRouting();

#pragma warning disable ASP0014
app.UseEndpoints(endpoints =>
{
    // endpoints.MapGet("map1", async (context) =>
    // {
    //     await context.Response.WriteAsync("In Map1");
    // });
    //
    // endpoints.MapPost("map2", async (context) =>
    // {
    //     await context.Response.WriteAsync("In Map2");
    // });

    endpoints.Map("files/{filename}.{extension}", async context =>
    {
        var fileneme = Convert.ToString(context.Request.RouteValues["filename"]);
        var extension = Convert.ToString(context.Request.RouteValues["extension"]);

        await context.Response.WriteAsync($"In Files - {fileneme} - {extension}");
    });

    endpoints.Map("employee/profile/{Employeename:length(4,7):alpha=merryMa}", async (context) =>
    {
        var employeename = context.Request.RouteValues["employeename"];
        await context.Response.WriteAsync($"employee - {employeename}");
    });

    endpoints.Map("product/map/{id:int:range(1,100)?}", async context =>
    {
        if (context.Request.RouteValues.TryGetValue("id", out var idObj))
        {
            var id = Convert.ToInt32(context.Request.RouteValues["id"]);
            id++;
            await context.Response.WriteAsync($"id++ : {id} - In Product Map");
        }
        else
        {
            await context.Response.WriteAsync("pleas input id ");
        }
    });

    //Eg: daily - digest - report/ {reportdate}
    endpoints.Map("daily-digest-report/{reportdate:datetime}", async context =>
    {
        var time = Convert.ToDateTime(context.Request.RouteValues["reportdate"]);
        await context.Response.WriteAsync($"In Daily Digest Report - {time.ToShortDateString()}");
    });

    //Ed: cities/cityId
    endpoints.Map("cities/{cityId:guid}", async context =>
    {
        var cityId = Guid.Parse(Convert.ToString(context.Request.RouteValues["cityId"])!);
        await context.Response.WriteAsync($"In Cities - {cityId}");
    });

    //sales-report/2030/apr
    endpoints.Map(("sales-report/{year:int:min(1900)}/{mouth:months}"), async contxt =>
    {
        var year = Convert.ToInt32(contxt.Request.RouteValues["year"]);
        var mouth = Convert.ToString(contxt.Request.RouteValues["mouth"]);
        await contxt.Response.WriteAsync($"In Sales Report - {year} - {mouth}");
    });
});
#pragma warning restore ASP0014

app.Run(async context => { await context.Response.WriteAsync(context.Request.Path); });
app.Run();