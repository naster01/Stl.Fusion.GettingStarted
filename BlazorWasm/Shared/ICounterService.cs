namespace BlazorWasm.Shared;

public interface ICounterService
{
    Task<int> Get();
    Task Increment();
    Task Reset();
}