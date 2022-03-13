using BlazorWasm.Shared;
using Microsoft.AspNetCore.Mvc;
using Stl.Fusion.Server;

namespace BlazorWasm.Server.Controllers;

[Route("api/[controller]/[action]")]
[ApiController, JsonifyErrors]
public class CounterController : ControllerBase, ICounterService
{
    private readonly ICounterService _counterService;

    public CounterController(ICounterService counterService)
    {
        _counterService = counterService;
    }

    [HttpGet, Publish]
    public Task<(int, DateTime)> Get() => _counterService.Get();

    [HttpPost]
    public Task Increment() => _counterService.Increment();
}
