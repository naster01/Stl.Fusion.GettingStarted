using BlazorWasm.Shared;
using Stl.Fusion;

namespace BlazorWasm.Server.Services;

public class CounterService : ICounterService
{
    private readonly object _lock = new();
    private int _count;
    
    [ComputeMethod]
    public virtual Task<int> Get()
    {
        lock (_lock)
        {
            return Task.FromResult(_count);
        }
    }

    public Task Increment()
    {
        lock (_lock)
        {
            ++_count;
        }
        using (Computed.Invalidate())
            Get();
        return Task.CompletedTask;
    }

    public Task Reset()
    {
        lock (_lock)
        {
            _count = 0;
        }
        using (Computed.Invalidate())
            Get();
        return Task.CompletedTask;
    }
}