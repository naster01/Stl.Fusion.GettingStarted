using RestEase;

namespace BlazorWasm.Client.RestDefenitions;

[BasePath("counter")]
public interface ICounterServiceDef
{
    [Get("get")]
    Task<int> Get();

    [Post("increment")]
    Task Increment();
    
    [Post("reset")]
    Task Reset();
}