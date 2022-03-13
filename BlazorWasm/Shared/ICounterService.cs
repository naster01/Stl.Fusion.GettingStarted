namespace BlazorWasm.Shared;

public interface ICounterService
{
    Task<(int, DateTime)> Get();
    Task Increment();
}