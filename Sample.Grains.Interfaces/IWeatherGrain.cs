using Orleans;
using Sample.Models;
using System.Collections.Immutable;
using System.Threading.Tasks;

namespace Sample.Grains
{
    public interface IWeatherGrain : IGrainWithGuidKey
    {
        Task<ImmutableList<WeatherInfo>> GetForecastAsync();
    }
}