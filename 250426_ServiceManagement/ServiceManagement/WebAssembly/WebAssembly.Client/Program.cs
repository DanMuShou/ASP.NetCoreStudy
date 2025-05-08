using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebAssembly.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.Services.AddSingleton<ContainerStorage>();

await builder.Build().RunAsync();

//11
