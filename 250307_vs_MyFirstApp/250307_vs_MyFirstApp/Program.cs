using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (HttpContext context) =>
{
    var streamReader = new StreamReader(context.Request.Body);
    var body = await streamReader.ReadToEndAsync();

    var query = QueryHelpers.ParseQuery(body);
    context.Request.Headers.Connection = "text/html";

    if (query.TryGetValue("firstName", out var value))
    {
        foreach (var item in value)
        {
            await context.Response.WriteAsync($"<h1>Hello {item}</h1>");
        }
    }

    if (query.TryGetValue("age", out var values))
    {
        foreach (var item in value)
        {
            await context.Response.WriteAsync($"<h1>Hello {item}</h1>");
        }
    }

    await context.Response.WriteAsync(body);

    //context.Response.Headers["Content-Type"] = "text/html";
    //if (context.Request.Headers.ContainsKey("User-Agent"))
    //{
    //    var userAgen = context.Request.Headers["User-Agent"];
    //    await context.Response.WriteAsync($"<h1>Your user agent is {userAgen}</h1>");
    //    var other = context.Request.Headers["AuthorizationKey"];
    //    await context.Response.WriteAsync($"<p>Your AuthorizationKey is {other}</p>");
    //}
    //context.Response.Headers["MyKey"] = "my value";
    //context.Response.Headers["Server"] = "MyServer";
    //context.Response.Headers["Content-Type"] = "text/html";
    //var path = context.Request.Path;
    //var method = context.Request.Method;
    //context.Response.Headers["Content-Type"] = "text/html";
    //await context.Response.WriteAsync($"<p>{path}</p>");
    //await context.Response.WriteAsync($"<p>{method}</p>");
    //await context.Response.WriteAsync("<h1>Hello Chrome</h1>");
    //await context.Response.WriteAsync("<h2>Hello Chrome * 2</h2>");
    //context.Response.Headers["Conten-Type"] = "text/html";
    //if (context.Request.Method == "GET")
    //{
    //    if (context.Request.Query.ContainsKey("id"))
    //    {
    //        var id = context.Request.Query["id"];
    //        await context.Response.WriteAsync($"<p>Your id is {id}</p>");
    //    }
    //    if (context.Request.Query.ContainsKey("name"))
    //    {
    //        var name = context.Request.Query["name"];
    //        await context.Response.WriteAsync($"<p>Your name is {name}</p>");
    //    }
    //}
});

app.Run();