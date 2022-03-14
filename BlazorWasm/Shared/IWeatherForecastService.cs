using Stl.Fusion;

namespace BlazorWasm.Shared;

public interface IWeatherForecastService
{
    [ComputeMethod]
    Task<WeatherForecast[]> GetForecast(DateTime startDate, CancellationToken cancellationToken = default);
}
