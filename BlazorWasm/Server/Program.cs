using System.Reflection;
using BlazorWasm.Server.Services;
using BlazorWasm.Shared;
using Stl.Fusion;
using Stl.Fusion.Bridge;
using Stl.Fusion.Server;

var builder = WebApplication.CreateBuilder(args);

// Fusion
var fusion = builder.Services.AddFusion();
var fusionServer = fusion.AddWebServer();
//builder.Services.AddSingleton(new Publisher.Options() {Id = "p-d174ad6f-bd86-4bcc-84c5-5199c03a6ae8"});

// Fusion Services
fusion.AddComputeService<ICounterService, CounterService>();

// Web
//builder.Services.AddRouting();

// builder.Services // Register Replica Service controllers
//     .AddMvc() 
//     .AddApplicationPart(Assembly.GetExecutingAssembly());
// builder.Services.AddServerSideBlazor();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSwaggerDocument();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
}

app.UseWebSockets(new WebSocketOptions() {
    KeepAliveInterval = TimeSpan.FromSeconds(30), // You can change this
});

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseOpenApi();
app.UseSwaggerUi3();

app.MapRazorPages();
app.UseEndpoints(endpoints => {
    endpoints.MapFusionWebSocketServer();
    endpoints.MapControllers();
});
app.MapFallbackToFile("index.html");

app.Run();
