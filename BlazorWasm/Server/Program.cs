using BlazorWasm.Server.Services;
using BlazorWasm.Shared;
using Stl.Fusion;
using Stl.Fusion.Blazor;
using Stl.Fusion.Server;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

// Fusion
var fusion = builder.Services.AddFusion();
var fusionServer = fusion.AddWebServer();

// Fusion Services
fusion.AddBlazorUIServices();
fusion.AddComputeService<ICounterService, CounterService>();
fusion.AddComputeService<IWeatherForecastService, WeatherForecastService>();

services.AddControllersWithViews();
services.AddRazorPages();

services.AddEndpointsApiExplorer();
services.AddSwaggerDocument();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseWebAssemblyDebugging();
    app.UseOpenApi();
    app.UseSwaggerUi3();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseWebSockets(new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(30),
});

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapFusionWebSocketServer();
app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();