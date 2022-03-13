using BlazorWasm.Client;
using BlazorWasm.Client.RestDefenitions;
using BlazorWasm.Shared;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Stl.DependencyInjection;
using Stl.Fusion;
using Stl.Fusion.Blazor;
using Stl.Fusion.Client;
using Stl.Fusion.UI;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var baseUri = new Uri(builder.HostEnvironment.BaseAddress);
var apiBaseUri = new Uri($"{baseUri}api/");

// Fusion services
var fusion = builder.Services.AddFusion();
fusion.AddBlazorUIServices();
var fusionClient = fusion.AddRestEaseClient(
    (c, o) =>
    {
        o.BaseUri = baseUri;
    }).ConfigureHttpClientFactory(
    (c, name, o) => {
        // This code configures any HttpClient, so if you use a few of them
        // you may add some extra logic to ensure their BaseAddress-es
        // are properly set here
        var isFusionClient = (name ?? "").Contains("FusionClient");
        var clientBaseUri = isFusionClient ? baseUri : apiBaseUri;
        o.HttpClientActions.Add(client => client.BaseAddress = clientBaseUri);
    });
fusionClient.AddReplicaService<ICounterService, ICounterServiceDef>();
builder.Services.AddTransient<IUpdateDelayer>(c => new UpdateDelayer(c.UICommandTracker(), 0.1));


var host = builder.Build();
// Blazor host doesn't start IHostedService-s by default,
// so let's start them "manually" here
await host.Services.HostedServices().Start();
await host.RunAsync();