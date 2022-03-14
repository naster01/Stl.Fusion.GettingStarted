using BlazorWasm.Client;
using BlazorWasm.Client.Services;
using BlazorWasm.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stl.Fusion;
using Stl.Fusion.Blazor;
using Stl.Fusion.Client;
using Stl.Fusion.Extensions;
using Stl.Fusion.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var services = builder.Services;
var baseUri = new Uri(builder.HostEnvironment.BaseAddress);
var apiBaseUri = new Uri($"{baseUri}api/");

// Fusion services
var fusion = services.AddFusion();
var fusionClient = fusion.AddRestEaseClient(
    (c, o) => {
        o.BaseUri = baseUri;
        o.IsLoggingEnabled = true;
        o.IsMessageLoggingEnabled = false;
    }).ConfigureHttpClientFactory(
    (c, name, o) => {
        var isFusionClient = (name ?? "").StartsWith("Stl.Fusion");
        var clientBaseUri = isFusionClient ? baseUri : apiBaseUri;
        o.HttpClientActions.Add(client => client.BaseAddress = clientBaseUri);
    });

// Fusion services
fusion.AddFusionTime();

// Fusion services clients
fusionClient.AddReplicaService<ICounterService, ICounterClientDef>();
fusionClient.AddReplicaService<IWeatherForecastService, IWeatherForecastClientDef>();

// Fusion Blazor UI services
fusion.AddBlazorUIServices();

builder.Services.AddTransient<IUpdateDelayer>(c => new UpdateDelayer(c.UICommandTracker(), 0.5));

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();