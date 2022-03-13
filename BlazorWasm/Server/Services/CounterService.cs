using BlazorWasm.Shared;
using Stl.Fusion;

namespace BlazorWasm.Server.Services;

public class CounterService : ICounterService
{
    private readonly object _lock = new();
    private int _count;
    private DateTime _changeTime = DateTime.Now;

    [ComputeMethod(KeepAliveTime = 9999999)]
    public virtual Task<(int, DateTime)> Get()
    {
        lock (_lock)
        {
            return Task.FromResult((_count, _changeTime));
        }
    }

    public Task Increment()
    {
        lock (_lock)
        {
            ++_count;
            _changeTime = DateTime.Now;
        }
        using (Computed.Invalidate())
            Get();
        return Task.CompletedTask;
    }
}